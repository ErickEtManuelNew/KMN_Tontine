using KMN_Tontine.Blazor.UI.Services.Base;

namespace KMN_Tontine.Blazor.UI.Services
{
    public interface IAuthService
    {
        Task<SimpleResponse> RegisterAsync(RegisterDTO model);
        Task<TokenResponse> LoginAsync(LoginDTO model);
        Task LogoutAsync();
    }
} 