using AutoMapper;
using KMN_Tontine.Domain.Entities;
using KMN_Tontine.Application.DTOs.Responses;
using KMN_Tontine.Application.DTOs.Requests;

namespace KMN_Tontine.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapping CreateMemberRequest -> Member
            CreateMap<CreateMemberRequest, Member>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString())) // Générer un ID unique
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}")) // Concaténer FirstName et LastName
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email)) // Utiliser Email comme UserName
                .ForMember(dest => dest.JoinDate, opt => opt.MapFrom(src => DateTime.UtcNow)); // Définir JoinDate automatiquement

            // Mapping Member -> MemberResponse
            CreateMap<Member, MemberResponse>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role));

            // Mapping Account -> AccountResponse
            CreateMap<Account, AccountResponse>();

            // Mapping Tontine -> TontineResponse
            CreateMap<Tontine, TontineResponse>();

            // Mapping Transaction -> TransactionResponse
            CreateMap<Transaction, TransactionResponse>();

            // Mapping PaymentPromise -> PaymentPromiseResponse
            CreateMap<PaymentPromise, PaymentPromiseResponse>()
                .ForMember(dest => dest.IsFulfilled, opt => opt.MapFrom(src => src.IsFulfilled));

            // Mapping RefreshToken -> TokenResponse
            CreateMap<RefreshToken, TokenResponse>()
                .ForMember(dest => dest.IsExpired, opt => opt.MapFrom(src => src.IsExpired));
        }
    }
}