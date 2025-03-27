using Blazored.LocalStorage;
using System.Net.Http.Headers;

namespace KMN_Tontine.Blazor.UI.Services.Base
{
    public class AuthenticationHeaderHandler : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorage;

        public AuthenticationHeaderHandler(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>("accessToken");
                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }
            catch (Exception)
            {
                // Si l'accès au localStorage échoue (par exemple pendant le rendu statique),
                // on continue sans ajouter le token
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
} 