using KMN_Tontine.Application.Common;
using KMN_Tontine.Shared.DTOs.Requests;
using KMN_Tontine.Shared.DTOs.Responses;

namespace KMN_Tontine.Application.Interfaces
{
    public interface IAuthService
    {
        Task<SimpleResponse> RegisterAsync(RegisterRequest request);
        Task<TokenResponse> LoginAsync(LoginRequest request);
        Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request);
        Task<SimpleResponse> LogoutAsync(Guid memberId);
    }
}
