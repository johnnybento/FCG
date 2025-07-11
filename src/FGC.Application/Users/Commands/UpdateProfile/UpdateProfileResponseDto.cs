

namespace FGC.Application.Users.Commands.UpdateProfile;

/// <summary>
/// DTO de resposta após atualização de perfil.
/// </summary>
public record UpdateProfileResponseDto(
    Guid Id,
    string Nome,
    string Email
);
