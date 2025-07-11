

namespace FGC.Application.Users.Queries.GetUserById;

/// <summary>
/// DTO com as informações de perfil de um usuário.
/// </summary>
public record UserProfileDto(
    Guid Id,
    string Nome,
    string Email,
    string Role
);