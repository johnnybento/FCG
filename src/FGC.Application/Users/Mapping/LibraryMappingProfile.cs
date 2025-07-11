using AutoMapper;
using FGC.Application.Users.Queries.ListLibrary;
using FGC.Core.Sale.Entities;

namespace FGC.Application.Users.Mapping;

public class LibraryMappingProfile : Profile
{
    public LibraryMappingProfile()
    {
        CreateMap<JogoAdquirido, LibraryItemDto>();
    }
}