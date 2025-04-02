using KMN_Tontine.Application.Common;
using KMN_Tontine.Application.Interfaces;
using KMN_Tontine.Shared.DTOs.Requests;
using KMN_Tontine.Shared.DTOs.Responses;

using Microsoft.AspNetCore.Mvc;

namespace KMN_Tontine.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// Récupérer un compte par ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAccount(int id)
        {
            try
            {
                var result = await _accountService.GetAccountByIdAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new SimpleResponse { Success = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// Récupérer tous les comptes
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AccountResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAllAccounts()
        {
            var result = await _accountService.GetAllAccountsAsync();
            return result.Any() ? Ok(result) : NoContent();
        }

        /// <summary>
        /// Récupérer tous les comptes d'un membre
        /// </summary>
        [HttpGet("member/{memberId}")]
        [ProducesResponseType(typeof(IEnumerable<AccountResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAccountsByMember(Guid memberId)
        {
            var result = await _accountService.GetAccountsByMemberIdAsync(memberId);
            return Ok(result);
        }

        /// <summary>
        /// Créer un nouveau compte
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest request)
        {
            var result = await _accountService.CreateAccountAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Mettre à jour un compte existant
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAccount(int id, [FromBody] UpdateAccountRequest request)
        {
            var result = await _accountService.UpdateAccountAsync(id, request);
            return result.Success ? Ok(result) : NotFound(result);
        }

        /// <summary>
        /// Supprimer un compte
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var result = await _accountService.DeleteAccountAsync(id);
            return result.Success ? NoContent() : NotFound(result);
        }
    }
}
