using AutoMapper;
using FGC.Application.Sale.Commands.ComprarJogo;
using FGC.Core.Sale.Entities;

namespace FGC.Application.Sale.Mapping;

public class SalesMappingProfile : Profile
{
    public SalesMappingProfile()
    {
        CreateMap<JogoAdquirido, ComprarJogoResponseDto>();
    }
}