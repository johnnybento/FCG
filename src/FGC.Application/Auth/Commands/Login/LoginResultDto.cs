
namespace FGC.Application.Auth.Commands.Login;
/// <summary>
/// DTO retornado após autenticação bem-sucedida.
/// </summary>
public record LoginResultDto(
    string Token,
    Guid UsuarioId,
    string Email,
    string Role
);