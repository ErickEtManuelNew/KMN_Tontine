using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Blazored.LocalStorage;

using KMN_Tontine.Blazor.UI.Services.Base;

using Microsoft.AspNetCore.Components.Authorization;

namespace KMN_Tontine.Blazor.UI.Services
{
    public class AuthService(HttpClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authenticationStateProvider)
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ILocalStorageService _localStorage = localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider = authenticationStateProvider;

        public async Task<string> RegisterAsync(RegisterDTO model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/membres/register", model);

            if (response.IsSuccessStatusCode)
                return "Inscription réussie ! Vérifiez votre email.";
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return $"Erreur : {errorMessage}";
            }
        }

        public async Task<string> GetUserIdFromToken()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (string.IsNullOrEmpty(token)) return null;

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var userId = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            return userId;
        }

        public async Task<InscriptionMembreDto> GetUserInfo()
        {
            var userId = await GetUserIdFromToken();
            if (string.IsNullOrEmpty(userId)) return null;

            return await _httpClient.GetFromJsonAsync<InscriptionMembreDto>($"api/membres/{userId}");
        }

        public async Task<bool> LoginAsync(LoginDTO model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/membres/login", model);

            if (response.IsSuccessStatusCode)
            {
                await _localStorage.SetItemAsync("authToken", await response.Content.ReadAsStringAsync());
                await ((ApiAuthenticationStateProvider) _authenticationStateProvider).LoggedIn();
                return true;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return false;
            }
        }
    }
}
