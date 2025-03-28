// Services/CurrentUserService.cs
using System.Security.Claims;

using Microsoft.AspNetCore.Components.Authorization;

namespace KMN_Tontine.Blazor.UI.Services
{
    public class CurrentUserService
    {
        private readonly AuthenticationStateProvider _authStateProvider;

        public string? UserName { get; private set; }
        public string? FullName { get; private set; }
        public bool IsAdmin { get; private set; }
        public bool IsMember { get; private set; }

        public CurrentUserService(AuthenticationStateProvider authStateProvider)
        {
            _authStateProvider = authStateProvider;
        }

        public async Task LoadUserInfoAsync()
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            UserName = user.Identity?.Name;
            var firstName = user.FindFirst(c => c.Type == ClaimTypes.Name)?.Value ?? "";
            var lastName = user.FindFirst(c => c.Type == ClaimTypes.Surname)?.Value ?? "";
            FullName = $"{firstName} {lastName}".Trim() ?? "Utilisateur";
            IsAdmin = user.IsInRole("Admin");
            IsMember = user.IsInRole("Member");
        }
    }
}
