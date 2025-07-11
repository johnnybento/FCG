using AutoMapper;
using FGC.Application.Users.Commands.CreateUser;
using FGC.Core.Users.Entities;

namespace FGC.Application.Users.Mapping;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<Usuario, CreateUserResponseDto>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value));
    }
}