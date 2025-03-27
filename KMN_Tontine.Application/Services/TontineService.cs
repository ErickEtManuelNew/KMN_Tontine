using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using KMN_Tontine.Application.Common;
using KMN_Tontine.Application.DTOs.Requests;
using KMN_Tontine.Application.DTOs.Responses;
using KMN_Tontine.Application.Interfaces;
using KMN_Tontine.Domain.Entities;
using KMN_Tontine.Domain.Interfaces;

namespace KMN_Tontine.Application.Services
{
    public class TontineService : ITontineService
    {
        private readonly ITontineRepository _tontineRepository;
        private readonly IMapper _mapper;

        public TontineService(ITontineRepository tontineRepository, IMapper mapper)
        {
            _tontineRepository = tontineRepository;
            _mapper = mapper;
        }

        public async Task<TontineResponse> GetTontineByIdAsync(int id)
        {
            var tontine = await _tontineRepository.GetByIdAsync(id);
            if (tontine == null)
                throw new KeyNotFoundException("Tontine not found");

            return _mapper.Map<TontineResponse>(tontine);
        }

        public async Task<IEnumerable<TontineResponse>> GetAllTontinesAsync()
        {
            var tontines = await _tontineRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<TontineResponse>>(tontines);
        }

        public async Task<SimpleResponse> CreateTontineAsync(CreateTontineRequest request)
        {
            try
            {
                var tontine = _mapper.Map<Tontine>(request);
                tontine.CreationDate = DateTime.UtcNow; // Définir la date de création
                await _tontineRepository.AddAsync(tontine);

                return SimpleResponse.Ok("Tontine created successfully");
            }
            catch (Exception ex)
            {
                return SimpleResponse.Error($"Failed to create tontine: {ex.Message}");
            }
        }

        public async Task<SimpleResponse> UpdateTontineAsync(int id, UpdateTontineRequest request)
        {
            try
            {
                var tontine = await _tontineRepository.GetByIdAsync(id);
                if (tontine == null)
                    return SimpleResponse.Error("Tontine not found");

                // Mettre à jour les propriétés
                tontine.Name = request.Name ?? tontine.Name;
                tontine.Email = request.Email ?? tontine.Email;
                tontine.Address = request.Address ?? tontine.Address;
                tontine.IsActive = request.IsActive ?? tontine.IsActive;

                await _tontineRepository.UpdateAsync(tontine);
                return SimpleResponse.Ok("Tontine updated successfully");
            }
            catch (Exception ex)
            {
                return SimpleResponse.Error($"Failed to update tontine: {ex.Message}");
            }
        }

        public async Task<SimpleResponse> DeleteTontineAsync(int id)
        {
            try
            {
                var tontine = await _tontineRepository.GetByIdAsync(id);
                if (tontine == null)
                    return SimpleResponse.Error("Tontine not found");

                await _tontineRepository.DeleteAsync(id);
                return SimpleResponse.Ok("Tontine deleted successfully");
            }
            catch (Exception ex)
            {
                return SimpleResponse.Error($"Failed to delete tontine: {ex.Message}");
            }
        }
    }
}
