using AutoMapper;
using KMN_Tontine.Application.DTOs;
using KMN_Tontine.Application.Interfaces;
using KMN_Tontine.Domain.Entities;
using KMN_Tontine.Infrastructure.Repositories.Interfaces;

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KMN_Tontine.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly ICompteRepository _compteRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountService> _logger;

        public AccountService(
            ICompteRepository compteRepository,
            IMapper mapper,
            ILogger<AccountService> logger)
        {
            _compteRepository = compteRepository ?? throw new ArgumentNullException(nameof(compteRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<CompteDTO>> GetComptesByMembreIdAsync(string membreId)
        {
            try
            {
                _logger.LogInformation($"Récupération des comptes pour le membre {membreId}");
                var comptes = await _compteRepository.GetComptesByMembreIdAsync(membreId);
                var compteDtos = _mapper.Map<IEnumerable<CompteDTO>>(comptes);
                _logger.LogInformation($"Nombre de comptes récupérés : {compteDtos?.Count() ?? 0}");
                return compteDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erreur lors de la récupération des comptes pour le membre {membreId}");
                throw;
            }
        }

        public async Task<CompteDTO?> GetCompteByIdAsync(int id)
        {
            var compte = await _compteRepository.GetByIdAsync(id);
            return compte != null ? _mapper.Map<CompteDTO>(compte) : null;
        }

        public async Task<CompteDTO> CreateCompteAsync(CompteDTO compteDto)
        {
            var compte = _mapper.Map<Compte>(compteDto);
            var createdCompte = await _compteRepository.AddAsync(compte);
            return _mapper.Map<CompteDTO>(createdCompte);
        }

        public async Task UpdateCompteAsync(CompteDTO compteDto)
        {
            var compte = _mapper.Map<Compte>(compteDto);
            await _compteRepository.UpdateAsync(compte);
        }

        public async Task DeleteCompteAsync(int id)
        {
            await _compteRepository.DeleteAsync(id);
        }
    }
}
