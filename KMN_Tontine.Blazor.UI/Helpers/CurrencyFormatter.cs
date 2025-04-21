using KMN_Tontine.Shared.Options;

using Microsoft.Extensions.Options;

namespace KMN_Tontine.Blazor.UI.Helpers
{
    public class CurrencyFormatter(IOptions<CurrencyOptions> options)
    {
        private readonly CurrencyOptions _options = options.Value;

        public string Format(decimal value)
        {
            var formatted = value.ToString(_options.Format);
            return $"{formatted} {_options.Symbol}";
        }
    }
}
