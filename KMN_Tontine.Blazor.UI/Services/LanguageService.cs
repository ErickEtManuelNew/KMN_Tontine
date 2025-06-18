using System.Globalization;
using Blazored.LocalStorage;

namespace KMN_Tontine.Blazor.UI.Services
{
    public class LanguageService
    {
        private readonly ILocalStorageService _localStorage;
        private const string CultureKey = "culture";

        public event Action? OnChange;

        public LanguageService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task SetCultureAsync(string culture)
        {
            var ci = new CultureInfo(culture);
            CultureInfo.DefaultThreadCurrentCulture = ci;
            CultureInfo.DefaultThreadCurrentUICulture = ci;
            await _localStorage.SetItemAsync(CultureKey, culture);
            OnChange?.Invoke();
        }

        public async Task<string> GetCultureAsync()
        {
            var culture = await _localStorage.GetItemAsync<string>(CultureKey);
            return string.IsNullOrWhiteSpace(culture) ? "en-US" : culture;
        }
    }
}
