

namespace FGC.Application.Catalog.Commands.CreatePromotion;

public record CreatePromotionResponseDto(
    Guid Id,
    Guid JogoId,
    decimal Desconto,
    DateTime Inicio,
    DateTime Termino
);
