using KMN_Tontine.Application.Common;
using KMN_Tontine.Application.Interfaces;
using KMN_Tontine.Shared.DTOs.Requests;
using KMN_Tontine.Shared.DTOs.Responses;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KMN_Tontine.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentPromisesController : ControllerBase
    {
        private readonly IPaymentPromiseService _paymentPromiseService;

        public PaymentPromisesController(IPaymentPromiseService paymentPromiseService)
        {
            _paymentPromiseService = paymentPromiseService;
        }

        /// <summary>
        /// Récupérer une promesse de paiement par ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PaymentPromiseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPaymentPromise(int id)
        {
            try
            {
                var result = await _paymentPromiseService.GetPaymentPromiseByIdAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new SimpleResponse { Success = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// Récupérer toutes les promesses de paiement
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PaymentPromiseResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAllPaymentPromises()
        {
            var result = await _paymentPromiseService.GetAllPaymentPromisesAsync();
            return result.Any() ? Ok(result) : NoContent();
        }

        /// <summary>
        /// Créer une nouvelle promesse de paiement
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePaymentPromise([FromBody] CreatePaymentPromiseRequest request)
        {
            var result = await _paymentPromiseService.CreatePaymentPromiseAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Mettre à jour une promesse de paiement existante
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePaymentPromise(int id, [FromBody] UpdatePaymentPromiseRequest request)
        {
            var result = await _paymentPromiseService.UpdatePaymentPromiseAsync(id, request);
            return result.Success ? Ok(result) : NotFound(result);
        }

        /// <summary>
        /// Supprimer une promesse de paiement
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePaymentPromise(int id)
        {
            var result = await _paymentPromiseService.DeletePaymentPromiseAsync(id);
            return result.Success ? NoContent() : NotFound(result);
        }

        [HttpGet("by-account/{accountId:int}")]
        [ProducesResponseType(typeof(List<PaymentPromiseResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<PaymentPromiseResponse>>> GetByAccount(int accountId)
        {
            if (accountId <= 0)
                return BadRequest("Invalid account ID.");

            var result = await _paymentPromiseService.GetByAccountIdAsync(accountId);

            return Ok(result);
        }

        [HttpGet("by-member/{memberId}")]
        [ProducesResponseType(typeof(List<PaymentPromiseResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<PaymentPromiseResponse>>> GetByMember(Guid memberId)
        {
            var result = await _paymentPromiseService.GetByMemberIdAsync(memberId);

            if (result == null)
                result = new List<PaymentPromiseResponse>();

            return Ok(result);
        }
    }
}
