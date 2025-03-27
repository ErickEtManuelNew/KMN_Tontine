﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KMN_Tontine.Domain.Entities;
using KMN_Tontine.Domain.Interfaces;
using KMN_Tontine.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

namespace KMN_Tontine.Infrastructure.Repositories.Implementations
{
    public class PaymentPromiseRepository : IPaymentPromiseRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentPromiseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PaymentPromise>> GetAllAsync()
            => await _context.PaymentPromises.ToListAsync();

        public async Task<IEnumerable<PaymentPromise>> GetByMemberIdAsync(Guid memberId)
        {
            // Vérification de l'ID (optionnel mais recommandé)
            if (memberId == Guid.Empty)
                throw new ArgumentException("Member ID cannot be empty", nameof(memberId));

            return await _context.PaymentPromises
                .Where(pp => pp.MemberId == memberId.ToString())
                .OrderByDescending(pp => pp.PromiseDate) // Tri par date d'échéance
                .Include(pp => pp.Member)            // Chargement des données du membre
                .AsNoTracking()                      // Pour les requêtes en lecture seule
                .ToListAsync();
        }

        public async Task<PaymentPromise?> GetByIdAsync(int id)
            => await _context.PaymentPromises.FindAsync(id);

        public async Task AddAsync(PaymentPromise promise)
        {
            await _context.PaymentPromises.AddAsync(promise);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PaymentPromise promise)
        {
            _context.PaymentPromises.Update(promise);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var promise = await _context.PaymentPromises.FindAsync(id);
            if (promise is not null)
            {
                _context.PaymentPromises.Remove(promise);
                await _context.SaveChangesAsync();
            }
        }
    }
}
