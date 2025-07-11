using MediatR;

namespace FGC.Application.Users.Queries.GetUserById;

/// <summary>
/// Consulta para obter os detalhes de um usuário pelo Id.
/// </summary>
public record GetUserByIdQuery(Guid UsuarioId) : IRequest<UserProfileDto?>;