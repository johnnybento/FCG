namespace FGC.Application.Sale.Commands.ComprarJogo;

public record ComprarJogoResponseDto(
    Guid JogoId,
    decimal PrecoPago,
    DateTime DataHora
);