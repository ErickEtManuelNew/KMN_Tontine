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
        private readonly IClient _client;
        private readonly ILogger<CompteService> _logger;

        public CompteService(IClient client, ILogger<CompteService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<List<CompteDTO>> GetComptesAsync(string membreId)
        {
            try
            {
                _logger.LogInformation($"Tentative de récupération des comptes pour le membre {membreId}");

                var response = await _client.MembreAsync(membreId);

                return (List<CompteDTO>)(response ?? new List<CompteDTO>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception lors de la récupération des comptes");
                throw;
            }
        }

        public async Task<CompteDTO> GetCompteAsync(int compteId)
        {
            try
            {
                return await _client.ComptesAsync(compteId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception lors de la récupération du compte {compteId}");
                throw;
            }
        }

        public async Task CrediterCompteAsync(CreateTransactionDTO transaction)
        {
            try
            {
                await _client.CrediterAsync(transaction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception lors du crédit du compte");
                throw;
            }
        }
    }
} 