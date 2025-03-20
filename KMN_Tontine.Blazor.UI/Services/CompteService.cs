using KMN_Tontine.Blazor.UI.Services.Base;
using System.Net.Http.Json;

namespace KMN_Tontine.Blazor.UI.Services
{
    public interface ICompteService
    {
        Task<List<CompteDTO>> GetComptesAsync(string membreId);
        Task<CompteDTO> GetCompteAsync(int compteId);
        Task CrediterCompteAsync(CreateTransactionDTO transaction);
    }

    public class CompteService : ICompteService
    {
        private readonly HttpClient _httpClient;
        private readonly IClient _client;

        public CompteService(HttpClient httpClient, IClient client)
        {
            _httpClient = httpClient;
            _client = client;
        }

        public async Task<List<CompteDTO>> GetComptesAsync(string membreId)
        {
            var response = await _httpClient.GetFromJsonAsync<List<CompteDTO>>($"api/comptes/membre/{membreId}");
            return response ?? new List<CompteDTO>();
        }

        public async Task<CompteDTO> GetCompteAsync(int compteId)
        {
            var response = await _httpClient.GetFromJsonAsync<CompteDTO>($"api/comptes/{compteId}");
            return response ?? new CompteDTO();
        }

        public async Task CrediterCompteAsync(CreateTransactionDTO transaction)
        {
            await _client.CrediterAsync(transaction);
        }
    }
} 