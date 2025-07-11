using MediatR;

namespace FGC.Application.Users.Commands.ChangePassword;

/// <summary>
/// Command para alteração de senha de usuário.
/// </summary>
public record ChangePasswordCommand(
    Guid UsuarioId,
    string SenhaAtual,
    string NovaSenha
) : IRequest;