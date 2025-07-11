using AutoMapper;
using FGC.Core.Catalog.Repositories;
using MediatR;

namespace FGC.Application.Catalog.Queries.ListGames;

public class ListGamesQueryHandler : IRequestHandler<ListGamesQuery, List<GameDto>>
{
    private readonly IJogoRepository _jogoRepository;
    private readonly IMapper _mapper;

    public ListGamesQueryHandler(IJogoRepository jogoRepository, IMapper mapper)
    {
        _jogoRepository = jogoRepository;
        _mapper = mapper;
    }

    public async Task<List<GameDto>> Handle(ListGamesQuery request, CancellationToken cancellationToken)
    {
        var jogos = await _jogoRepository.ListAsync();
        return _mapper.Map<List<GameDto>>(jogos);
    }
}