using KMN_Tontine.Blazor.UI.Services.Base;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

using System.Net.Http.Json;

namespace KMN_Tontine.Blazor.UI.Services
{
    public interface ICompteService
    {
        Task<List<AccountResponse>> GetComptesAsync(string membreId);
        Task<AccountResponse> GetCompteAsync(int compteId);
        Task CrediterCompteAsync(CreateTransactionRequest transaction);
    }

    public class CompteService : ICompteService
    {
        private readonly ProtectedLocalStorage _protectedLocalStore;
        private readonly IClient _client;
        private readonly ILogger<CompteService> _logger;

        public CompteService(IClient client, ILogger<CompteService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<List<AccountResponse>> GetComptesAsync(string membreId)
        {
            try
            {
                _logger.LogInformation($"Tentative de récupération des comptes pour le membre {membreId}");

                var response = await _client.MemberAsync(Guid.Parse(membreId));

                return (List<AccountResponse>)(response ?? new List<AccountResponse>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception lors de la récupération des comptes");
                throw;
            }
        }

        public async Task<AccountResponse> GetCompteAsync(int compteId)
        {
            try
            {
                return await _client.AccountsGETAsync(compteId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception lors de la récupération du compte {compteId}");
                throw;
            }
        }

        public async Task CrediterCompteAsync(CreateTransactionRequest transaction)
        {
            try
            {
                //await _client.CrediterAsync(transaction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception lors du crédit du compte");
                throw;
            }
        }
    }
} 