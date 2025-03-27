using KMN_Tontine.Application.Common;
using KMN_Tontine.Application.DTOs.Requests;
using KMN_Tontine.Application.DTOs.Responses;
using KMN_Tontine.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KMN_Tontine.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembersController : ControllerBase
    {
        private readonly IMemberService _memberService;

        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        /// <summary>
        /// Récupérer un membre par ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MemberResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMember(Guid id)
        {
            try
            {
                var result = await _memberService.GetMemberByIdAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new SimpleResponse { Success = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// Récupérer tous les membres
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MemberResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAllMembers()
        {
            var result = await _memberService.GetAllMembersAsync();
            return result.Any() ? Ok(result) : NoContent();
        }

        /// <summary>
        /// Créer un nouveau membre
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateMember([FromBody] CreateMemberRequest request)
        {
            var result = await _memberService.CreateMemberAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Mettre à jour un membre existant
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SimpleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateMember(Guid id, [FromBody] UpdateMemberRequest request)
        {
            var result = await _memberService.UpdateMemberAsync(id, request);
            return result.Success ? Ok(result) : NotFound(result);
        }

        /// <summary>
        /// Supprimer un membre
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMember(Guid id)
        {
            var result = await _memberService.DeleteMemberAsync(id);
            return result.Success ? NoContent() : NotFound(result);
        }
    }
}
