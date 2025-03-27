﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KMN_Tontine.Domain.Entities;

namespace KMN_Tontine.Domain.Interfaces
{
    public interface IMemberRepository
    {
        Task<Member?> GetByIdAsync(Guid id);
        Task<Member?> GetByEmailAsync(string email);
        Task<IEnumerable<Member>> GetAllAsync();
        Task AddAsync(Member member);
        Task UpdateAsync(Member member);
        Task DeleteAsync(Guid id);
    }
}
