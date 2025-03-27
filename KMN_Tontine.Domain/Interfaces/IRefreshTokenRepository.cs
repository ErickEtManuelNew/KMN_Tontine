using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KMN_Tontine.Domain.Entities;

namespace KMN_Tontine.Domain.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task<IEnumerable<RefreshToken>> GetByMemberIdAsync(Guid memberId);
        Task AddAsync(RefreshToken refreshToken);
        Task DeleteAsync(string token);
        Task DeleteAllForUserAsync(Guid memberId);
    }
}
