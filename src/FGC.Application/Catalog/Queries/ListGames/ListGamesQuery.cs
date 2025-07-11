
using MediatR;

namespace FGC.Application.Catalog.Queries.ListGames;

public record ListGamesQuery() : IRequest<List<GameDto>>;