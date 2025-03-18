using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using KMN_Tontine.Application.DTOs;
using KMN_Tontine.Application.Interfaces;
using KMN_Tontine.Domain.Entities;
using KMN_Tontine.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

using Microsoft.Extensions.Configuration;

using Microsoft.IdentityModel.Tokens;

namespace KMN_Tontine.Application.Services
{
    public class MembreService : IMembreService
    {
        private readonly UserManager<Membre> _userManager;
        private readonly SignInManager<Membre> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public MembreService(UserManager<Membre> userManager, SignInManager<Membre> signInManager, IConfiguration configuration, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;
        }

        public async Task<MembreDTO> RegisterAsync(RegisterDTO registerDto)
        {
            var user = new Membre
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                Matricule = registerDto.Matricule,
                Nom = registerDto.Nom,
                Prenom = registerDto.Prenom,
                TypeMembre = registerDto.TypeMembre
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                throw new Exception("Échec de l'inscription : " + string.Join(", ", result.Errors));
            }

            return new MembreDTO
            {
                Id = user.Id,
                Matricule = user.Matricule,
                Nom = user.Nom,
                Prenom = user.Prenom,
                Email = user.Email,
                TypeMembre = user.TypeMembre
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
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id) // ✅ Ajoute cet identifiant
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
