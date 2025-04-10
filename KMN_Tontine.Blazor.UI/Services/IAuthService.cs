using KMN_Tontine.Blazor.UI.Services.Base;

namespace KMN_Tontine.Blazor.UI.Services
{
    public interface IAuthService
    {
        Task<SimpleResponse> RegisterAsync(RegisterRequest model);
        Task<TokenResponse> LoginAsync(LoginRequest model);
        Task LogoutAsync();
        Task<SimpleResponse> ConfirmEmailAsync(Guid userId, string token);
    }
} 