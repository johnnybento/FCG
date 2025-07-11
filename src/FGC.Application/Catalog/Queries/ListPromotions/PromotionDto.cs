
namespace FGC.Application.Catalog.Queries.ListPromotions;

public record PromotionDto(
    Guid Id,
    Guid JogoId,
    decimal Desconto,
    DateTime Inicio,
    DateTime Termino
);
