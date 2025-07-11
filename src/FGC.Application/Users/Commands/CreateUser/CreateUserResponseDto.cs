
namespace FGC.Application.Users.Commands.CreateUser;

public record CreateUserResponseDto(
    Guid Id,
    string Nome,
    string Email
);
