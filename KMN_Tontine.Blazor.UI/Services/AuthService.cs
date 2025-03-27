using Blazored.LocalStorage;

using KMN_Tontine.Blazor.UI.Services.Base;

using Microsoft.AspNetCore.Components.Authorization;

namespace KMN_Tontine.Blazor.UI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IClient _client;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthService(
            IClient client,
            ILocalStorageService localStorage,
            AuthenticationStateProvider authenticationStateProvider)
        {
            _client = client;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<SimpleResponse> RegisterAsync(RegisterRequest model)
        {
            return await _client.RegisterAsync(model);
        }

        public async Task<TokenResponse> LoginAsync(LoginRequest model)
        {
            var response = await _client.LoginAsync(model);
            
            if (response?.AccessToken != null)
            {
                await _localStorage.SetItemAsync("accessToken", response.AccessToken);
                await ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedIn();
            }
            
            return response;
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("accessToken");
            await ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedOut();
        }
    }
} 