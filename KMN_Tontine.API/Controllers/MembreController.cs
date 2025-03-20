using KMN_Tontine.Application.DTOs;
using KMN_Tontine.Application.DtosResponses;
using KMN_Tontine.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace KMN_Tontine.API.Controllers
{
    [Route("api/membres")]
    [ApiController]
    public class MembreController : ControllerBase
    {
        private readonly IMembreService _membreService;

        public MembreController(IMembreService membreService)
        {
            _membreService = membreService;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            try
            {
                var membre = await _membreService.RegisterAsync(registerDto);

                // Vérifie si l'inscription a réussi (par exemple, si `membre` n'est pas null)
                if (membre != null)
                {
                    return Created("", new SimpleResponse
                    {
                        Success = true,
                        Message = "Inscription réussie."
                    });
                }

                // En cas d'échec sans exception (ex. validation échouée)
                return BadRequest(new SimpleResponse
                {
                    Success = false,
                    Message = "L'inscription a échoué. Veuillez vérifier vos informations."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new SimpleResponse
                {
                    Success = false,
                    Message = $"Erreur : {ex.Message}"
                });
            }
        }

        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            try
            {
                var token = await _membreService.LoginAsync(loginDto);
                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Email ou mot de passe incorrect.");
            }
        }

        [HttpGet("{membreId}")]
        public async Task<IActionResult> GetMembreById(string membreId)
        {
            var membre = await _membreService.GetMembreByIdAsync(membreId);
            if (membre == null)
                return NotFound(new { message = "Membre introuvable." });

            return Ok(membre);
        }
    }
}
