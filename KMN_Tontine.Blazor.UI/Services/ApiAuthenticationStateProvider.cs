using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Blazored.LocalStorage;

namespace KMN_Tontine.Blazor.UI.Services;

public class ApiAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly JwtSecurityTokenHandler _tokenHandler = new();
    private bool _isInitialized = false;

    public ApiAuthenticationStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity());

        // 🔹 Empêcher l'accès au LocalStorage trop tôt (évite l'erreur JavaScript Interop)
        if (!_isInitialized)
        {
            await Task.Delay(500); // Petit délai pour éviter l'erreur
            await LoadUserFromStorage();
            _isInitialized = true;
        }

        return new AuthenticationState(user);
    }

    private async Task LoadUserFromStorage()
    {
        try
        {
            var savedToken = await _localStorage.GetItemAsync<string>("authToken");

            if (!string.IsNullOrEmpty(savedToken))
            {
                var tokenContent = _tokenHandler.ReadJwtToken(savedToken);

                if (tokenContent.ValidTo > DateTime.UtcNow)
                {
                    var claims = tokenContent.Claims;
                    var identity = new ClaimsIdentity(claims, "jwt");
                    var user = new ClaimsPrincipal(identity);

                    NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la récupération du token: {ex.Message}");
        }
    }

    public async Task LoggedIn()
    {
        await LoadUserFromStorage();
    }

    public async Task LoggedOut()
    {
        await _localStorage.RemoveItemAsync("authToken");
        var nobody = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(nobody)));
    }
}