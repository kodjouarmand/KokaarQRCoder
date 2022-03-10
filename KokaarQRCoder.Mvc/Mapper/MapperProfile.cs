using AutoMapper;
using KokaarQrCoder.Domain.Assemblers;
using KokaarQrCoder.Domain.Entities;

namespace KokaarQrCoder.Mvc.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Company, CompanyDto>();
            CreateMap<CompanyDto, Company>();

            CreateMap<SocialNetwork, SocialNetworkDto>();
            CreateMap<SocialNetworkDto, SocialNetwork>();

            CreateMap<SocialNetworkAccount, SocialNetworkAccountDto>();
            CreateMap<SocialNetworkAccountDto, SocialNetworkAccount>();

            CreateMap<Agency, AgencyDto>();
            CreateMap<AgencyDto, Agency>();

            CreateMap<Agent, AgentDto>();
            CreateMap<AgentDto, Agent>();
        }
    }
}
