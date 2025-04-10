using KMN_Tontine.Application.Common;
using KMN_Tontine.Application.Interfaces;
using KMN_Tontine.Shared.DTOs.Requests;
using KMN_Tontine.Shared.DTOs.Responses;

using Microsoft.AspNetCore.Mvc;

namespace KMN_Tontine.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Inscription d'un nouvel utilisateur
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Connexion d'un utilisateur
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Rafraîchir le token d'accès
        /// </summary>
        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var result = await _authService.RefreshTokenAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Déconnexion d'un utilisateur
        /// </summary>
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Logout([FromBody] Guid memberId)
        {
            var result = await _authService.LogoutAsync(memberId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Confirmer l'email d'un utilisateur
        /// </summary>
        /// <param name="userId">Identifiant unique de l'utilisateur</param>
        /// <param name="token">Token de confirmation d'email</param>
        /// <returns>Une réponse simple indiquant le succès ou l'échec de l'opération</returns>
        /// <response code="200">Email confirmé avec succès</response>
        /// <response code="400">Échec de la confirmation de l'email</response>
        [HttpPost("confirm-email")]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConfirmEmail([FromQuery] Guid userId, [FromQuery] string token)
        {
            var result = await _authService.ConfirmEmailAsync(userId, token);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
