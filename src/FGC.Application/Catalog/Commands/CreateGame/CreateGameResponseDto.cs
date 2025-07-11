namespace FGC.Application.Catalog.Commands.CreateGame;

public record CreateGameResponseDto(
    Guid Id,
    string Titulo,
    string Descricao,
    decimal Preco
);