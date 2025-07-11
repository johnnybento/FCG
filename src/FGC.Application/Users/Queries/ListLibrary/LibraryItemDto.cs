
namespace FGC.Application.Users.Queries.ListLibrary;

/// <summary>
/// DTO representando um item da biblioteca do usuário.
/// </summary>
public record LibraryItemDto(
    Guid JogoId,
    decimal PrecoPago,
    DateTime DataHora
);