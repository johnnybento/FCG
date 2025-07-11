
using MediatR;

namespace FGC.Application.Users.Commands.UpdateProfile;

/// <summary>
/// Command para atualização de perfil de usuário.
/// </summary>
public record UpdateProfileCommand(
    Guid UsuarioId,
    string Nome,
    string Email
) : IRequest<UpdateProfileResponseDto>;
