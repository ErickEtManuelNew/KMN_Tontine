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
    public class PaymentPromiseService : IPaymentPromiseService
    {
        private readonly IPaymentPromiseRepository _paymentPromiseRepository;
        private readonly IMapper _mapper;

        public PaymentPromiseService(IPaymentPromiseRepository paymentPromiseRepository, IMapper mapper)
        {
            _paymentPromiseRepository = paymentPromiseRepository;
            _mapper = mapper;
        }

        public async Task<PaymentPromiseResponse> GetPaymentPromiseByIdAsync(int id)
        {
            var promise = await _paymentPromiseRepository.GetByIdAsync(id);
            if (promise == null)
                throw new KeyNotFoundException("Payment promise not found");

            return _mapper.Map<PaymentPromiseResponse>(promise);
        }

        public async Task<IEnumerable<PaymentPromiseResponse>> GetAllPaymentPromisesAsync()
        {
            var promises = await _paymentPromiseRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PaymentPromiseResponse>>(promises);
        }

        public async Task<SimpleResponse> CreatePaymentPromiseAsync(CreatePaymentPromiseRequest request)
        {
            try
            {
                var promise = _mapper.Map<PaymentPromise>(request);
                await _paymentPromiseRepository.AddAsync(promise);

                return SimpleResponse.Ok("Payment promise created successfully");
            }
            catch (Exception ex)
            {
                return SimpleResponse.Error($"Failed to create payment promise: {ex.Message}");
            }
        }

        public async Task<SimpleResponse> UpdatePaymentPromiseAsync(int id, UpdatePaymentPromiseRequest request)
        {
            try
            {
                var promise = await _paymentPromiseRepository.GetByIdAsync(id);
                if (promise == null)
                    return SimpleResponse.Error("Payment promise not found");

                // Mettre à jour les propriétés
                promise.AmountPromised = request.AmountPromised;
                promise.PromiseDate = request.PromiseDate;

                await _paymentPromiseRepository.UpdateAsync(promise);
                return SimpleResponse.Ok("Payment promise updated successfully");
            }
            catch (Exception ex)
            {
                return SimpleResponse.Error($"Failed to update payment promise: {ex.Message}");
            }
        }

        public async Task<SimpleResponse> DeletePaymentPromiseAsync(int id)
        {
            try
            {
                var promise = await _paymentPromiseRepository.GetByIdAsync(id);
                if (promise == null)
                    return SimpleResponse.Error("Payment promise not found");

                await _paymentPromiseRepository.DeleteAsync(id);
                return SimpleResponse.Ok("Payment promise deleted successfully");
            }
            catch (Exception ex)
            {
                return SimpleResponse.Error($"Failed to delete payment promise: {ex.Message}");
            }
        }

        public async Task<List<PaymentPromiseResponse>> GetByAccountIdAsync(int accountId)
        {
            var list = await _paymentPromiseRepository.GetByAccountIdAsync(accountId);
            return _mapper.Map<List<PaymentPromiseResponse>>(list);
        }

        public async Task<List<PaymentPromiseResponse>> GetByMemberIdAsync(Guid memberId)
        {
            var list = await _paymentPromiseRepository.GetByMemberIdAsync(memberId);
            return _mapper.Map<List<PaymentPromiseResponse>>(list);
        }
    }
}
