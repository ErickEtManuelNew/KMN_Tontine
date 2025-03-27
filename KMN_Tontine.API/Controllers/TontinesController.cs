using KMN_Tontine.Application.Common;
using KMN_Tontine.Application.DTOs.Requests;
using KMN_Tontine.Application.DTOs.Responses;
using KMN_Tontine.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace KMN_Tontine.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TontinesController : ControllerBase
    {
        private readonly ITontineService _tontineService;

        public TontinesController(ITontineService tontineService)
        {
            _tontineService = tontineService;
        }

        /// <summary>
        /// Récupérer une tontine par ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TontineResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTontine(int id)
        {
            try
            {
                var result = await _tontineService.GetTontineByIdAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new SimpleResponse { Success = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// Récupérer toutes les tontines
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TontineResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAllTontines()
        {
            var result = await _tontineService.GetAllTontinesAsync();
            return result.Any() ? Ok(result) : NoContent();
        }

        /// <summary>
        /// Créer une nouvelle tontine
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTontine([FromBody] CreateTontineRequest request)
        {
            var result = await _tontineService.CreateTontineAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Mettre à jour une tontine existante
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTontine(int id, [FromBody] UpdateTontineRequest request)
        {
            var result = await _tontineService.UpdateTontineAsync(id, request);
            return result.Success ? Ok(result) : NotFound(result);
        }

        /// <summary>
        /// Supprimer une tontine
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTontine(int id)
        {
            var result = await _tontineService.DeleteTontineAsync(id);
            return result.Success ? NoContent() : NotFound(result);
        }
    }
}
