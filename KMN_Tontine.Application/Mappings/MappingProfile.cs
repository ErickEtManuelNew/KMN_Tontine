using AutoMapper;

using KMN_Tontine.Domain.Entities;
using KMN_Tontine.Shared.DTOs.Requests;
using KMN_Tontine.Shared.DTOs.Responses;

namespace KMN_Tontine.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapping CreateMemberRequest -> Member
            CreateMap<CreateMemberRequest, Member>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString())) // G�n�rer un ID unique
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}")) // Concat�ner FirstName et LastName
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email)) // Utiliser Email comme UserName
                .ForMember(dest => dest.JoinDate, opt => opt.MapFrom(src => DateTime.UtcNow)); // D�finir JoinDate automatiquement

            // Mapping Member -> MemberResponse
            CreateMap<Member, MemberResponse>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth));

            // Mapping Account -> AccountResponse
            CreateMap<Account, AccountResponse>()
                .ForMember(dest => dest.MemberFullName, opt => opt.MapFrom(src => src.Member != null ? src.Member.FullName : ""))
                //.ForMember(dest => dest.TontineId, opt => opt.MapFrom(src => src.Tontine != null ? src.Tontine.Name : ""))
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment ?? ""));

            // Mapping Tontine -> TontineResponse
            CreateMap<Tontine, TontineResponse>();

            // Mapping Transaction -> TransactionResponse
            CreateMap<Transaction, TransactionResponse>();

            // Mapping PaymentPromise -> PaymentPromiseResponse
            CreateMap<CreatePaymentPromiseRequest, PaymentPromise>()
                .ForMember(dest => dest.FulfilledDate, opt => opt.Ignore()) // non fourni � la cr�ation
                .ForMember(dest => dest.IsFulfilled, opt => opt.Ignore())   // calcul� automatiquement
                .ForMember(dest => dest.Account, opt => opt.Ignore())       // navigation, non mapp�e
                .ForMember(dest => dest.Member, opt => opt.Ignore());       // navigation, non mapp�e

            CreateMap<PaymentPromise, PaymentPromiseResponse>()
                .ForMember(dest => dest.AccountName,
                           opt => opt.MapFrom(src => src.Account.Type.ToString())); // ou .Name si string

            CreateMap<CreateTransactionRequest, Transaction>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(_ => DateTime.UtcNow));

            // Mapping RefreshToken -> TokenResponse
            CreateMap<RefreshToken, TokenResponse>()
                .ForMember(dest => dest.IsExpired, opt => opt.MapFrom(src => src.IsExpired));
        }
    }
}