using KMN_Tontine.Application.DTOs;
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
    }
}
