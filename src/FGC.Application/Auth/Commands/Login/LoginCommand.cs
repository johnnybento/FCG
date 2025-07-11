using MediatR;

namespace FGC.Application.Auth.Commands.Login;

/// <summary>
/// Command para autenticação de usuário.
/// </summary>
public record LoginCommand(
    string Email,
    string Senha
) : IRequest<LoginResultDto>;
