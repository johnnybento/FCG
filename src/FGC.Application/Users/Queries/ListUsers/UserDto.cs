
namespace FCG.Application.Users.Queries.ListarUsuarios;


public record UserDto(
    Guid Id,
    string Nome,
    string Email,
    string Role
);
