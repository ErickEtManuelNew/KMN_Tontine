using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace KMN_Tontine.Blazor.UI.Services;

public class JwtAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ProtectedSessionStorage _sessionStorage;
    private ClaimsPrincipal _currentUser = new(new ClaimsIdentity());

    public JwtAuthenticationStateProvider(ProtectedSessionStorage sessionStorage)
    {
        _sessionStorage = sessionStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var tokenResult = await _sessionStorage.GetAsync<string>("jwtToken");

        if (tokenResult.Success && !string.IsNullOrEmpty(tokenResult.Value))
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(tokenResult.Value);

            var identity = new ClaimsIdentity(jwt.Claims, "jwt");
            _currentUser = new ClaimsPrincipal(identity);
        }

        return new AuthenticationState(_currentUser);
    }

    public async Task MarkUserAsAuthenticated(string token)
    {
        await _sessionStorage.SetAsync("jwtToken", token);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task MarkUserAsLoggedOut()
    {
        await _sessionStorage.DeleteAsync("jwtToken");
        _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}