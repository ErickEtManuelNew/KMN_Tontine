using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

using KMN_Tontine.Application.DTOs;
using KMN_Tontine.Application.Interfaces;
using KMN_Tontine.Domain.Entities;
using KMN_Tontine.Infrastructure.Data;
using KMN_Tontine.Infrastructure.Repositories.Implementations;
using KMN_Tontine.Infrastructure.Repositories.Interfaces;

using Microsoft.AspNetCore.Identity;

using Microsoft.Extensions.Configuration;

using Microsoft.IdentityModel.Tokens;

namespace KMN_Tontine.Application.Services
{
    public class MembreService : IMembreService
    {
        private readonly UserManager<Membre> _userManager;
        private readonly IMembreRepository _membreRepository;
        private readonly IAssociationRepository _associationRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public MembreService(UserManager<Membre> userManager, IMembreRepository membreRepository, IConfiguration configuration, IAssociationRepository associationRepository, IMapper mapper)
        {
            _userManager = userManager;
            _membreRepository = membreRepository;
            _associationRepository = associationRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<MembreDTO> RegisterAsync(RegisterDTO registerDto)
        {
            var user = new Membre
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                Nom = registerDto.Nom,
                Prenom = registerDto.Prenom,
                Type = registerDto.TypeMembre,
                AssociationId = 1 // 🔥 Association par défaut
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                throw new Exception("Échec de l'inscription : " + string.Join(", ", result.Errors));
            }

            return new MembreDTO
            {
                Id = user.Id,
                Nom = user.Nom,
                Prenom = user.Prenom,
                Email = user.Email,
                Type = user.Type
            };
        }

        public async Task<string> LoginAsync(LoginDTO loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                throw new UnauthorizedAccessException("Email ou mot de passe incorrect.");
            }

            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(Membre user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var jwtKey = jwtSettings["Key"];

            if (string.IsNullOrEmpty(jwtKey))
            {
                jwtKey = Environment.GetEnvironmentVariable("Jwt__Key");
            }

            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("❌ ERREUR : La clé JWT est introuvable !");
            }

            var key = Encoding.UTF8.GetBytes(jwtKey);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Nom),
                new Claim(ClaimTypes.Surname, user.Prenom),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id) // ✅ Ajoute cet identifiant
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"], // 🔥 L'émetteur du token
                audience: jwtSettings["Audience"], // 🔥 L'audience du token
                claims: claims,
                expires: DateTime.UtcNow.AddHours(Convert.ToDouble(jwtSettings["ExpireHours"])), // 🔥 Expiration
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<Membre> InscrireMembreAsync(InscriptionMembreDto dto)
        {
            // Vérifier si l’association existe
            var association = await _associationRepository.GetByIdAsync(dto.AssociationId);
            if (association == null)
                throw new Exception("L'association spécifiée n'existe pas.");

            // Vérifier si l’email est déjà utilisé via UserManager
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
                throw new Exception("Cet email est déjà utilisé.");

            // Création du membre
            var membre = _mapper.Map<Membre>(dto);
            membre.UserName = dto.Email; // ASP.NET Identity requiert un UserName
            membre.EmailConfirmed = false; // L'email doit être confirmé

            var result = await _userManager.CreateAsync(membre, dto.Password);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            // Génération du token de confirmation d'email
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(membre);

            // Envoi d’un email avec le lien de confirmation (à implémenter)
            // _emailService.SendEmailConfirmation(dto.Email, token);

            return membre;
        }

        public async Task<MembreDTO?> GetMembreByIdAsync(string membreId)
        {
            var membre = await _membreRepository.GetByMembreIdAsync(membreId);

            return _mapper.Map<MembreDTO>(membre);
        }
    }
}
