using Application.User.commands.login;
using AutoMapper;


namespace Application.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.User_account,UserAccountLoginOutput>();
        }
    }
}