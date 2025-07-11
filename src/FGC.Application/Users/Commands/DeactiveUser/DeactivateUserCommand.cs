
using MediatR;

namespace FGC.Application.Users.Commands.DeactiveUser;

/// <summary>
/// Command para desativar um usuário.
/// </summary>
public record DeactivateUserCommand(Guid UsuarioId) : IRequest;
