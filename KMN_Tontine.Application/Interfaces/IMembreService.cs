using KMN_Tontine.Application.DTOs;
using KMN_Tontine.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMN_Tontine.Application.Interfaces
{
    public interface IMembreService
    {
        Task<MembreDTO> RegisterAsync(RegisterDTO registerDto);
        Task<string> LoginAsync(LoginDTO loginDto);
        Task<Membre> InscrireMembreAsync(InscriptionMembreDto dto);
        Task<Membre?> GetMembreByIdAsync(string membreId);
    }
}
