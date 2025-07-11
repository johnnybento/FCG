using MediatR;

namespace FGC.Application.Catalog.Commands.CreateGame;

public record CreateGameCommand(
    string Titulo,
    string Descricao,
    decimal Preco
) : IRequest<CreateGameResponseDto>;
