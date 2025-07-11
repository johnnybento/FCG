using AutoMapper;
using FGC.Application.Users.Commands.UpdateProfile;
using FGC.Core.Users.Entities;

namespace FGC.Application.Users.Mapping;
public class UpdateProfileMappingProfile : Profile
{
    public UpdateProfileMappingProfile()
    {
        CreateMap<Usuario, UpdateProfileResponseDto>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value));
    }
}