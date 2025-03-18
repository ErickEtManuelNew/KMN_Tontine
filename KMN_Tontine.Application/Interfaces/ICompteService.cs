using KMN_Tontine.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMN_Tontine.Application.Interfaces
{
    public interface ICompteService
    {
        Task<List<CompteDTO>> GetAllComptesAsync();
        Task<CompteDTO> GetCompteByIdAsync(int id);
        Task<CompteDTO> CreateCompteAsync(CreateCompteDTO compteDto);
    }
}
