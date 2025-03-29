using KMN_Tontine.Application.Common;
using KMN_Tontine.Application.DTOs.Requests;
using KMN_Tontine.Application.DTOs.Responses;
using KMN_Tontine.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace KMN_Tontine.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        /// <summary>
        /// Récupérer une transaction par ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTransaction(int id)
        {
            try
            {
                var result = await _transactionService.GetTransactionByIdAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new SimpleResponse { Success = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// Récupérer toutes les transactions
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TransactionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAllTransactions()
        {
            var result = await _transactionService.GetAllTransactionsAsync();
            return result.Any() ? Ok(result) : NoContent();
        }

        /// <summary>
        /// Créer une nouvelle transaction
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionRequest request)
        {
            var result = await _transactionService.CreateTransactionAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Mettre à jour une transaction existante
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTransaction(int id, [FromBody] UpdateTransactionRequest request)
        {
            var result = await _transactionService.UpdateTransactionAsync(id, request);
            return result.Success ? Ok(result) : NotFound(result);
        }

        /// <summary>
        /// Supprimer une transaction
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var result = await _transactionService.DeleteTransactionAsync(id);
            return result.Success ? NoContent() : NotFound(result);
        }

        [HttpGet("by-account/{accountId:int}")]
        [ProducesResponseType(typeof(List<TransactionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<TransactionResponse>>> GetByAccount(int accountId)
        {
            if (accountId <= 0)
                return BadRequest("Invalid account ID.");

            var result = await _transactionService.GetTransactionsByAccountIdAsync(accountId);

            return Ok(result);
        }

        [HttpGet("by-member/{memberId}")]
        [ProducesResponseType(typeof(List<TransactionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<TransactionResponse>>> GetByMember(string memberId)
        {
            var result = await _transactionService.GetTransactionsByMemberIdAsync(memberId);

            if (result == null || !result.Any())
                return NotFound($"No transactions found for member ID {memberId}.");

            return Ok(result);
        }
    }
}
