using KMN_Tontine.Application.DTOs;
using KMN_Tontine.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KMN_Tontine.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _compteService;
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(IAccountService compteService, ILogger<AccountsController> logger)
        {
            _compteService = compteService ?? throw new ArgumentNullException(nameof(compteService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Récupère tous les comptes d'un membre spécifique
        /// </summary>
        /// <param name="membreId">L'identifiant unique du membre</param>
        /// <returns>La liste des comptes du membre</returns>
        /// <response code="200">Retourne la liste des comptes</response>
        /// <response code="401">Non autorisé</response>
        /// <response code="404">Aucun compte trouvé pour ce membre</response>
        [HttpGet("membre/{membreId}")]
        [ProducesResponseType(typeof(IEnumerable<CompteDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<CompteDTO>>> GetComptesByMembre(string membreId)
        {
            try
            {
                _logger.LogInformation($"Tentative de récupération des comptes pour le membre {membreId}");
                var comptes = await _compteService.GetComptesByMembreIdAsync(membreId);
                
                if (!comptes.Any())
                {
                    _logger.LogWarning($"Aucun compte trouvé pour le membre {membreId}");
                    return NotFound($"Aucun compte trouvé pour le membre {membreId}");
                }

                _logger.LogInformation($"Comptes récupérés avec succès pour le membre {membreId}");
                return Ok(comptes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erreur lors de la récupération des comptes pour le membre {membreId}");
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Une erreur est survenue lors de la récupération des comptes");
            }
        }

        /// <summary>
        /// Récupère un compte spécifique par son ID
        /// </summary>
        /// <param name="id">L'identifiant unique du compte</param>
        /// <returns>Le compte demandé</returns>
        /// <response code="200">Retourne le compte demandé</response>
        /// <response code="401">Non autorisé</response>
        /// <response code="404">Compte non trouvé</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CompteDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CompteDTO>> GetCompte(int id)
        {
            var compte = await _compteService.GetCompteByIdAsync(id);
            if (compte == null)
            {
                return NotFound($"Compte avec l'ID {id} non trouvé");
            }
            return Ok(compte);
        }
    }
} 