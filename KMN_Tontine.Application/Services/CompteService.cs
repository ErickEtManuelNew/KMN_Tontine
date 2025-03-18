using AutoMapper;

using KMN_Tontine.Application.DTOs;
using KMN_Tontine.Application.Interfaces;
using KMN_Tontine.Domain.Entities;
using KMN_Tontine.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

namespace KMN_Tontine.Application.Services
{
    public class CompteService : ICompteService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CompteService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CompteDTO>> GetAllComptesAsync()
        {
            var comptes = await _context.Comptes.ToListAsync();
            return _mapper.Map<List<CompteDTO>>(comptes);
        }

        public async Task<CompteDTO> GetCompteByIdAsync(int id)
        {
            var compte = await _context.Comptes.FindAsync(id);
            return _mapper.Map<CompteDTO>(compte);
        }

        public async Task<CompteDTO> CreateCompteAsync(CreateCompteDTO compteDto)
        {
            var compte = _mapper.Map<Compte>(compteDto);
            _context.Comptes.Add(compte);
            await _context.SaveChangesAsync();
            return _mapper.Map<CompteDTO>(compte);
        }
    }
}
