using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using KMN_Tontine.Application.Common;
using KMN_Tontine.Application.Interfaces;
using KMN_Tontine.Domain.Entities;
using KMN_Tontine.Domain.Interfaces;
using KMN_Tontine.Shared.DTOs.Requests;
using KMN_Tontine.Shared.DTOs.Responses;
using KMN_Tontine.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using KMN_Tontine.Infrastructure.Data;

namespace KMN_Tontine.Application.Services
{
    public class PaymentPromiseService : IPaymentPromiseService
    {
        private readonly IPaymentPromiseRepository _paymentPromiseRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public PaymentPromiseService(
            IPaymentPromiseRepository paymentPromiseRepository,
            IAccountRepository accountRepository,
            ITransactionRepository transactionRepository,
            INotificationService notificationService,
            IMapper mapper,
            ApplicationDbContext context)
        {
            _paymentPromiseRepository = paymentPromiseRepository;
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _notificationService = notificationService;
            _mapper = mapper;
            _context = context;
        }

        public async Task<PaymentPromiseResponse> GetPaymentPromiseByIdAsync(int id)
        {
            var promise = await _paymentPromiseRepository.GetByIdAsync(id);
            if (promise == null)
                throw new KeyNotFoundException("Payment promise not found");

            var response = _mapper.Map<PaymentPromiseResponse>(promise);
            response.MemberFullName = promise.Member.FullName;
            response.AmountPaid = CalculateAmountPaid(promise);

            return response;
        }

        public async Task<IEnumerable<PaymentPromiseResponse>> GetAllPaymentPromisesAsync()
        {
            var promises = await _paymentPromiseRepository.GetAllAsync();
            var responses = _mapper.Map<List<PaymentPromiseResponse>>(promises);

            foreach (var response in responses)
            {
                var promise = promises.First(p => p.Id == response.Id);
                response.MemberFullName = promise.Member.FullName;
                response.AmountPaid = CalculateAmountPaid(promise);
            }

            return responses;
        }

        public async Task<SimpleResponse> CreatePaymentPromiseAsync(CreatePaymentPromiseRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Reference))
                    return SimpleResponse.Error("La référence est requise");

                // Vérifier si la référence existe déjà
                var existingPromise = await _context.PaymentPromises
                    .FirstOrDefaultAsync(p => p.Reference == request.Reference);
                    
                if (existingPromise != null)
                    return SimpleResponse.Error("Cette référence existe déjà");
                
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

                // Mettre à jour les propriétés de base
                promise.PromiseDate = request.PromiseDate;

                // Mettre à jour les montants promis pour chaque compte
                foreach (var accountUpdate in request.Accounts)
                {
                    var promiseAccount = promise.PaymentPromiseAccounts
                        .FirstOrDefault(pa => pa.AccountId == accountUpdate.AccountId);

                    if (promiseAccount != null)
                    {
                        promiseAccount.AmountPromised = accountUpdate.AmountPromised;
                    }
                }

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
            var responses = _mapper.Map<List<PaymentPromiseResponse>>(list);
            foreach (var response in responses)
            {
                var promise = list.First(p => p.Id == response.Id);
                response.MemberFullName = promise.Member.FullName;
                response.AmountPaid = CalculateAmountPaid(promise);
            }
            return responses;
        }

        public async Task<List<PaymentPromiseResponse>> GetByMemberIdAsync(Guid memberId)
        {
            var list = await _paymentPromiseRepository.GetByMemberIdAsync(memberId);
            var responses = _mapper.Map<List<PaymentPromiseResponse>>(list);
            foreach (var response in responses)
            {
                var promise = list.First(p => p.Id == response.Id);
                response.MemberFullName = promise.Member.FullName;
                response.AmountPaid = CalculateAmountPaid(promise);
            }
            return responses;
        }

        public async Task<SimpleResponse> ValidatePaymentPromiseAsync(ValidatePaymentPromiseRequest request)
        {
            try
            {
                var promise = await _paymentPromiseRepository.GetByIdAsync(request.PaymentPromiseId);
                if (promise == null)
                    return SimpleResponse.Error("Promesse de paiement non trouvée");

                if (promise.IsFulfilled)
                    return SimpleResponse.Error("Cette promesse a déjà été validée");

                var totalAmountPromised = promise.TotalAmountPromised;
                var amountPaid = request.AmountPaid;
                var privateAccount = await _accountRepository.GetPrivateAccountAsync(promise.MemberId);

                if (privateAccount == null)
                    return SimpleResponse.Error("Compte privé non trouvé");

                var totalAvailable = amountPaid + privateAccount.Balance;

                if (totalAvailable >= totalAmountPromised)
                {
                    // Montant suffisant avec le compte privé
                    foreach (var promiseAccount in promise.PaymentPromiseAccounts)
                    {
                        if(promiseAccount.AmountPromised <= 0)
                            continue;

                        // Créer une transaction pour chaque compte associé à la promesse
                        var account = await _accountRepository.GetByIdAsync(promiseAccount.AccountId);
                        if (account == null)
                        {
                            return SimpleResponse.Error($"Compte non trouvé pour la promesse #{promise.Id}");
                        }

                        // Créer la transaction pour le compte concerné
                        var transaction = new Transaction
                        {
                            AccountId = account.Id,
                            Amount = promiseAccount.AmountPromised,
                            Type = TransactionType.Credit,
                            Description = $"Paiement promesse #{promise.Id}",
                            CreatedDate = request.PaymentDate,
                            PaymentPromiseId = promise.Id,
                            TontineId = account.TontineId
                        };
                        await _transactionRepository.AddAsync(transaction);
                        account.Balance += promiseAccount.AmountPromised;
                        await _accountRepository.UpdateAsync(account);
                    }

                    // Calculer le montant restant après ventilation
                    var remainingAmount = totalAvailable - totalAmountPromised;

                    // Si surplus, le virer sur le compte privé
                    if (remainingAmount > 0)
                    {
                        var surplusTransaction = new Transaction
                        {
                            AccountId = privateAccount.Id,
                            Amount = remainingAmount,
                            Type = TransactionType.Credit,
                            Description = $"Surplus paiement promesse #{promise.Id}",
                            CreatedDate = request.PaymentDate,
                            PaymentPromiseId = promise.Id,
                            TontineId = privateAccount.TontineId
                        };
                        await _transactionRepository.AddAsync(surplusTransaction);
                        privateAccount.Balance = remainingAmount;
                        await _accountRepository.UpdateAsync(privateAccount);
                    }
                    else
                    {
                        // Ajuster le solde du compte privé
                        privateAccount.Balance = 0;
                        await _accountRepository.UpdateAsync(privateAccount);
                    }

                    // Marquer la promesse comme validée car elle est totalement honorée
                    promise.FulfilledDate = request.PaymentDate;
                    await _paymentPromiseRepository.UpdateAsync(promise);
                }
                else
                {
                    // Montant insuffisant même avec le compte privé, tout va sur le compte privé
                    var transaction = new Transaction
                    {
                        AccountId = privateAccount.Id,
                        Amount = amountPaid,
                        Type = TransactionType.Credit,
                        Description = $"Paiement partiel promesse #{promise.Id}",
                        CreatedDate = request.PaymentDate,
                        TontineId = privateAccount.TontineId,
                        PaymentPromiseId = promise.Id
                    };
                    await _transactionRepository.AddAsync(transaction);
                    privateAccount.Balance += amountPaid;
                    await _accountRepository.UpdateAsync(privateAccount);

                    // Envoyer un message au membre pour le reste attendu
                    var remainingAmount = totalAmountPromised - totalAvailable;
                    var notificationResult = await _notificationService.SendPaymentReminderAsync(
                        promise.MemberId,
                        remainingAmount,
                        promise.Reference
                    );

                    if (!notificationResult.Success)
                    {
                        // On continue même si la notification échoue
                        Console.WriteLine($"Erreur lors de l'envoi de la notification : {notificationResult.Message}");
                    }

                    // Ne pas marquer la promesse comme validée car elle n'est pas totalement honorée
                    return SimpleResponse.Ok($"Paiement partiel enregistré. Il reste {remainingAmount} à payer.");
                }

                return SimpleResponse.Ok("Paiement validé avec succès");
            }
            catch (Exception ex)
            {
                return SimpleResponse.Error($"Erreur lors de la validation : {ex.Message}");
            }
        }

        private decimal CalculateAmountPaid(PaymentPromise promise)
        {
            if (!promise.IsFulfilled)
            {
                return promise.Transactions.Any() ? promise.Transactions.Sum(t => t.Amount) : 0;
            }
            else
            {
                var surplus = promise.Transactions
                    .FirstOrDefault(t => t.Description.StartsWith("Surplus paiement"))?.Amount ?? 0;

                return promise.Transactions.Sum(t => t.Amount) - surplus;
            }
        }

    }
}
