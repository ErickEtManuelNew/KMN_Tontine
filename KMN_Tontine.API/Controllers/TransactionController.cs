using KMN_Tontine.Application.DTOs;
using KMN_Tontine.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KMN_Tontine.API.Controllers
{
    [Authorize]
    [Route("api/transactions")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("{membreId}")]
        public async Task<IActionResult> GetTransactions(string membreId)
        {
            if (string.IsNullOrEmpty(membreId))
            {
                return BadRequest("L'identifiant du membre est requis.");
            }

            var transactions = await _transactionService.GetTransactionsAsync(membreId);
            if (transactions == null || !transactions.Any())
            {
                return NotFound("Aucune transaction trouvée pour ce membre.");
            }

            return Ok(transactions);
        }

        [HttpPost("crediter")]
        public async Task<IActionResult> Crediter([FromBody] CreateTransactionDTO dto)
        {
            if (dto.Montant <= 0)
                return BadRequest("Le montant doit être supérieur à zéro.");

            await _transactionService.AjouterTransactionAsync(dto.MembreId, dto.CompteId, dto.Montant,Domain.Enums.TypeTransaction.Versement);
            return Ok(); // CreatedAtAction(nameof(GetTransactions), new { membreId = transaction.MembreId }, transaction);
        }
    }
}
