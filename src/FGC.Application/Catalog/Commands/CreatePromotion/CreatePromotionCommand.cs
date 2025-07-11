
using MediatR;

namespace FGC.Application.Catalog.Commands.CreatePromotion;

public record CreatePromotionCommand(
    Guid JogoId,
    decimal Desconto,
    DateTime Inicio,
    DateTime Termino
) : IRequest<CreatePromotionResponseDto>;
