using MediatR;

namespace FGC.Application.Sale.Commands.ComprarJogo;

public record ComprarJogoCommand(Guid UsuarioId, Guid JogoId) : IRequest<ComprarJogoResponseDto>;