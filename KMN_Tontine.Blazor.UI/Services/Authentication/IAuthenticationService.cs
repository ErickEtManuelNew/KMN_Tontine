using KMN_Tontine.Blazor.UI.Services.Base;

namespace KMN_Tontine.Blazor.UI.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<Response<TokenResponse>> AuthenticateAsync(LoginRequest loginModel);
        Task LogoutAsync();
    }
}
