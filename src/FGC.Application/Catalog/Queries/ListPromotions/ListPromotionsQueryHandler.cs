using AutoMapper;
using FGC.Core.Catalog.Repositories;
using MediatR;

namespace FGC.Application.Catalog.Queries.ListPromotions;

public class ListPromotionsQueryHandler : IRequestHandler<ListPromotionsQuery, List<PromotionDto>>
{
    private readonly IPromocaoRepository _promocaoRepository;
    private readonly IMapper _mapper;

    public ListPromotionsQueryHandler(IPromocaoRepository promocaoRepository, IMapper mapper)
    {
        _promocaoRepository = promocaoRepository;
        _mapper = mapper;
    }

    public async Task<List<PromotionDto>> Handle(ListPromotionsQuery request, CancellationToken cancellationToken)
    {
        var promocoes = await _promocaoRepository.GetActiveByJogoIdAsync(request.JogoId, DateTime.UtcNow);
        return _mapper.Map<List<PromotionDto>>(promocoes);
    }
}