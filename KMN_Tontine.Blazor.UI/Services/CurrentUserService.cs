// Services/CurrentUserService.cs
using System.Security.Claims;

using Blazored.LocalStorage;

using Microsoft.AspNetCore.Components.Authorization;

namespace KMN_Tontine.Blazor.UI.Services
{
    public class CurrentUserService(AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage)
    {
        private readonly AuthenticationStateProvider _authStateProvider = authStateProvider;
        private readonly ILocalStorageService _localStorage = localStorage;

        public string? UserName { get; private set; }
        public string? FullName { get; private set; }
        public bool IsAdmin { get; private set; }
        public bool IsMember { get; private set; }
        public Guid UserId { get; private set; }
        public string? AccessToken { get; private set; }

        public async Task LoadUserInfoAsync()
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            UserName = user.Identity?.Name;
            UserId = Guid.Parse(user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            var firstName = user.FindFirst(c => c.Type == ClaimTypes.Name)?.Value ?? "";
            var lastName = user.FindFirst(c => c.Type == ClaimTypes.Surname)?.Value ?? "";
            FullName = $"{firstName} {lastName}".Trim() ?? "Utilisateur";
            IsAdmin = user.IsInRole("Admin") || user.IsInRole("SuperAdmin");
            IsMember = user.IsInRole("Member");

            // 🔑 Lire le token JWT (accessToken) si besoin
            AccessToken = await _localStorage.GetItemAsync<string>("accessToken");
        }
    }
}
