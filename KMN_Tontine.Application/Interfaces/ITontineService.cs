using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KMN_Tontine.Application.Common;
using KMN_Tontine.Application.DTOs.Requests;
using KMN_Tontine.Application.DTOs.Responses;

namespace KMN_Tontine.Application.Interfaces
{
    public interface ITontineService
    {
        Task<TontineResponse> GetTontineByIdAsync(int id);
        Task<IEnumerable<TontineResponse>> GetAllTontinesAsync();
        Task<SimpleResponse> CreateTontineAsync(CreateTontineRequest request);
        Task<SimpleResponse> UpdateTontineAsync(int id, UpdateTontineRequest request);
        Task<SimpleResponse> DeleteTontineAsync(int id);
    }
}
