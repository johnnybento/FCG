using FCG.Application.Users.Queries.ListarUsuarios;
using MediatR;

namespace FCG.Application.Users.Queries.ListUsers;

public record ListUsersQuery() : IRequest<List<UserDto>>;