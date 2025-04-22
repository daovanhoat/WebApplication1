using AutoMapper;
using WebApplication1.Migrations;
using WebApplication1.Models;

namespace WebApplication1.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<AccountModel, AccountDto>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => "******"))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => "hidden@example.com"));
            CreateMap<RegisterDto, AccountModel>();
        }
    }
}
