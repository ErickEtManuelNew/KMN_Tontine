using KMN_Tontine.Application.DTOs;
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
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            try
            {
                var membre = await _membreService.RegisterAsync(registerDto);
                return Ok(membre);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

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

        [HttpPost("inscrire")]
        public async Task<IActionResult> InscrireMembre([FromBody] InscriptionMembreDto dto)
        {
            try
            {
                var membre = await _membreService.InscrireMembreAsync(dto);
                return CreatedAtAction(nameof(GetMembreById), new { membreId = membre.Id }, new { message = "Inscription réussie. Vérifiez votre email pour confirmer votre compte." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
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
