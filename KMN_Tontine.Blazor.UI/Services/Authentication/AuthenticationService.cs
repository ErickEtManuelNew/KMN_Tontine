using Blazored.LocalStorage;
using KMN_Tontine.Blazor.UI.Services.Base;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;

namespace KMN_Tontine.Blazor.UI.Services.Authentication
{
    public class AuthenticationService : BaseHttpService, IAuthenticationService
    {
        private readonly IClient httpClient;
        private readonly ILocalStorageService localStorage;
        private readonly AuthenticationStateProvider authenticationStateProvider;

        public AuthenticationService(IClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authenticationStateProvider)
            : base(httpClient, localStorage)
        {
            this.httpClient = httpClient;
            this.localStorage = localStorage;
            this.authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<Response<TokenResponse>> AuthenticateAsync(LoginRequest loginModel)
        {
            Response<TokenResponse> response;
            try
            {
                var result = await httpClient.LoginAsync(loginModel);
                response = new Response<TokenResponse>
                {
                    Data = result,
                    Success = true,
                };
                //Store Token
                await localStorage.SetItemAsync("accessToken", result.AccessToken);

                //Change auth state of app
                await ((ApiAuthenticationStateProvider)authenticationStateProvider).LoggedIn();
            }
            catch (ApiException exception)
            {
                response = ConvertApiExceptions<TokenResponse>(exception);
            }

            return response;
        }

        public async Task Logout()
        {
            await ((ApiAuthenticationStateProvider)authenticationStateProvider).LoggedOut();
        }
    }
}
