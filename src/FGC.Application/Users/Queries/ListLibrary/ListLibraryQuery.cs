

using MediatR;

namespace FGC.Application.Users.Queries.ListLibrary;

/// <summary>
/// Consulta para listar todos os jogos adquiridos por um usuário.
/// </summary>
public record ListLibraryQuery(Guid UsuarioId) : IRequest<List<LibraryItemDto>>;
