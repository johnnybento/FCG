
using AutoMapper;
using FGC.Application.Catalog.Commands.CreateGame;
using FGC.Application.Catalog.Commands.CreatePromotion;
using FGC.Application.Catalog.Queries.ListGames;
using FGC.Application.Catalog.Queries.ListPromotions;
using FGC.Core.Catalog.Entities;

namespace FGC.Application.Catalog.Mapping;

public class CatalogMappingProfile : Profile
{
    public CatalogMappingProfile()
    {
        CreateMap<Jogo, CreateGameResponseDto>();
        CreateMap<Promocao, CreatePromotionResponseDto>();
        CreateMap<Jogo, GameDto>();
        CreateMap<Promocao, PromotionDto>();
    }
}