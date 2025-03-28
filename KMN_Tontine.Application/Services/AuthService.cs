using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using KMN_Tontine.Application.Common;
using KMN_Tontine.Application.DTOs.Requests;
using KMN_Tontine.Application.DTOs.Responses;

using KMN_Tontine.Application.Interfaces;
using KMN_Tontine.Domain.Entities;
using KMN_Tontine.Domain.Enums;

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

        public AuthService(IAccountService accountService, UserManager<Member> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
            _accountService = accountService;
        }

        public async Task<SimpleResponse> RegisterAsync(RegisterRequest request)
        {
            // Créer un jeton de confirmation
            var confirmationToken = Guid.NewGuid().ToString();

            var user = new Member
            {                
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                Role = request.Role,
                UserName = request.Email,
                Email = request.Email,
                FullName = request.FirstName + request.LastName,
                PasswordHash = request.Password,
                ConfirmationCode = confirmationToken,
                EmailConfirmed = false
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return new SimpleResponse()
                {
                    Success = false,
                    Message = string.Join(" | ", result.Errors.Select(e => e.Description))
                };
            }
            else
            {
                // Créer les comptes associés à l'utilisateur
                // Affecter tous les types de comptes (enum) à l'utilisateur
                var res = _accountService.CreateAccountForMemberAsync(user.Id).Result;
                if (res.Success)
                    return new SimpleResponse()
                    {
                        Success = true,
                        Message = string.Empty
                    };
                else
                    return new SimpleResponse()
                    {
                        Success = false,
                        Message =res.Message
                    };
            }
        }

        public async Task<TokenResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                throw new UnauthorizedAccessException("Invalid email or password");

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
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()) // Ajoutez des rôles si nécessaire
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new TokenResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = "refresh_token_placeholder" // Générez un vrai refresh token si nécessaire
            };
        }
    }
}
