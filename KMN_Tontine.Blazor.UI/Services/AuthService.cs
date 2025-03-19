using System.ComponentModel.DataAnnotations;

using KMN_Tontine.Blazor.UI.Services.Base;

namespace KMN_Tontine.Blazor.UI.Services
{
    public class AuthService(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;

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
    }
}
