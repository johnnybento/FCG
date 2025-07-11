

using MediatR;

namespace FGC.Application.Users.Commands.CreateUser;

public record CreateUserCommand(
    string Nome,
    string Email,
    string Senha
) : IRequest<CreateUserResponseDto>;