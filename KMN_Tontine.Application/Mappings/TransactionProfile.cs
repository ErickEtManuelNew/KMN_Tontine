using AutoMapper;
using KMN_Tontine.Application.DTOs;
using KMN_Tontine.Domain.Entities;

namespace KMN_Tontine.Application.Mappings
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<Transaction, TransactionDTO>();
            CreateMap<CreateTransactionDTO, Transaction>();
        }
    }
}
