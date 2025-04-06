using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using KMN_Tontine.Application.Common;

using KMN_Tontine.Application.Interfaces;
using KMN_Tontine.Domain.Entities;
using KMN_Tontine.Shared.Enums;
using KMN_Tontine.Shared.DTOs.Requests;
using KMN_Tontine.Shared.DTOs.Responses;
using KMN_Tontine.Infrastructure.Interface;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace KMN_Tontine.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<Member> _userManager;
        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(IUnitOfWork unitOfWork, IAccountService accountService, UserManager<Member> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
            _accountService = accountService;
            _unitOfWork = unitOfWork;
        }

        public async Task<SimpleResponse> RegisterAsync(RegisterRequest request)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Vérifier si l'email existe déjà
                var existingUser = await _userManager.FindByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    await _unitOfWork.RollbackAsync();
                    return new SimpleResponse
                    {
                        Success = false,
                        Message = "Un compte existe déjà avec cette adresse email."
                    };
                }

                // Créer un jeton de confirmation (sera utilisé plus tard pour la validation email)
                var confirmationToken = Guid.NewGuid().ToString();

                var user = new Member
                {                
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    DateOfBirth = request.DateOfBirth,
                    UserName = request.Email,
                    Email = request.Email,
                    FullName = $"{request.FirstName} {request.LastName}".Trim(),
                    JoinDate = DateTime.UtcNow,
                    ConfirmationCode = confirmationToken,
                    // État initial du membre
                    EmailConfirmed = false,     // L'email n'est pas encore confirmé
                    IsActive = false,           // En attente d'approbation par un admin
                    LockoutEnabled = false,     // Pas encore rejeté
                    PhoneNumberConfirmed = false
                };

                // Créer l'utilisateur avec Identity
                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                {
                    await _unitOfWork.RollbackAsync();
                    return new SimpleResponse
                    {
                        Success = false,
                        Message = string.Join(" | ", result.Errors.Select(e => e.Description))
                    };
                }

                // Créer les comptes associés à l'utilisateur
                var accountResult = await _accountService.CreateAccountForMemberAsync(user.Id);
                if (!accountResult.Success)
                {
                    await _unitOfWork.RollbackAsync();
                    return new SimpleResponse
                    {
                        Success = false,
                        Message = accountResult.Message
                    };
                }

                // Ajouter le rôle Member par défaut
                // Note: même si l'utilisateur a demandé un autre rôle, on force le rôle Member
                var roleResult = await _userManager.AddToRoleAsync(user, RoleType.Member.ToString());
                if (!roleResult.Succeeded)
                {
                    await _unitOfWork.RollbackAsync();
                    return new SimpleResponse
                    {
                        Success = false,
                        Message = string.Join(" | ", roleResult.Errors.Select(e => e.Description))
                    };
                }

                await _unitOfWork.CommitAsync();

                // Retourner un message de succès personnalisé
                return new SimpleResponse
                {
                    Success = true,
                    Message = "Inscription réussie ! Votre compte est en attente d'approbation par un administrateur. " +
                             "Vous recevrez un email de confirmation une fois votre compte approuvé."
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return new SimpleResponse 
                { 
                    Success = false, 
                    Message = $"Une erreur est survenue lors de l'inscription : {ex.Message}" 
                };
            }
        }

        public async Task<TokenResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            
            if (user == null)
            {
                return new TokenResponse { IsSuccess = false, Message = "Invalid email or password" };
            }
            
            // Vérifier si le compte est verrouillé (rejeté)
            if (user.LockoutEnabled && user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTimeOffset.UtcNow)
            {
                return new TokenResponse { IsSuccess = false, Message = "Your account has been rejected" };
            }
            
            // Vérifier si le compte est actif (approuvé)
            if (!user.IsActive)
            {
                return new TokenResponse { IsSuccess = false, Message = "Your account is pending approval by an administrator" };
            }
            
            // Vérifier si l'email est confirmé (après approbation)
            if (!user.EmailConfirmed)
            {
                return new TokenResponse { IsSuccess = false, Message = "Please confirm your email address" };
            }
            
            // La vérification du mot de passe
            var result = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!result)
            {
                return new TokenResponse { IsSuccess = false, Message = "Invalid email or password" };
            }

            return GenerateToken(user);
        }

        public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            // Implémentez ici la logique de rafraîchissement du token
            throw new NotImplementedException();
        }

        public async Task<SimpleResponse> LogoutAsync(Guid memberId)
        {
            // Implémentez ici la logique de déconnexion (par exemple, invalider le token)
            return SimpleResponse.Ok("Logged out successfully");
        }

        private TokenResponse GenerateToken(Member user)
        {
            var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, role) // Ajoutez des rôles si nécessaire
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(6),
                signingCredentials: creds
            );

            return new TokenResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = "refresh_token_placeholder", // Générez un vrai refresh token si nécessaire
                IsSuccess = true,
            };
        }
    }
}
