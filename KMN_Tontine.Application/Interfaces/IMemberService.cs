using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KMN_Tontine.Application.Common;
using KMN_Tontine.Application.DTOs.Requests;
using KMN_Tontine.Application.DTOs.Responses;

namespace KMN_Tontine.Application.Interfaces
{
    public interface IMemberService
    {
        Task<MemberResponse> GetMemberByIdAsync(Guid id);
        Task<IEnumerable<MemberResponse>> GetAllMembersAsync();
        Task<SimpleResponse> CreateMemberAsync(CreateMemberRequest request);
        Task<SimpleResponse> UpdateMemberAsync(Guid id, UpdateMemberRequest request);
        Task<SimpleResponse> DeleteMemberAsync(Guid id);
        Task<SimpleResponse> DeactivateMemberAsync(Guid id);
    }
}
