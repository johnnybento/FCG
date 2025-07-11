namespace FGC.Application.Catalog.Queries.ListGames;

public record GameDto(
    Guid Id,
    string Titulo,
    string Descricao,
    decimal Preco
);