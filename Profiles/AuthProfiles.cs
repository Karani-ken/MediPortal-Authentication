using MediPortal_AuthService.Models;
using MediPortal_AuthService.Models.Dtos;
using AutoMapper;

namespace Authentication_service.Profiles
{
    public class AuthProfiles:Profile
    {
        public AuthProfiles()
        {
            CreateMap<RegisterRequestDto, User>()
            .ForMember(dest => dest.UserName, u => u.MapFrom(reg => reg.Email));

            CreateMap<User, UserDto>().ReverseMap();
        }

    }
}
