using AutoMapper;
using KMN_Tontine.Application.DTOs;
using KMN_Tontine.Domain.Entities;

namespace KMN_Tontine.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Compte, CompteDTO>().ReverseMap();
        }
    }
} 