using System.Text.Json.Serialization;

namespace KMN_Tontine.Blazor.UI.Services.Base
{
    public class CompteDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nom")]
        public string Nom { get; set; } = string.Empty;

        [JsonPropertyName("solde")]
        public decimal Solde { get; set; }

        [JsonPropertyName("notes")]
        public string? Notes { get; set; }
    }
} 