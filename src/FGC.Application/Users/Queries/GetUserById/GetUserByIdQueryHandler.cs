
using FGC.Core.Users.Repositories;
using MediatR;

namespace FGC.Application.Users.Queries.GetUserById;
public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserProfileDto?>
{
    private readonly IUserRepository _usuarioRepository;

    public GetUserByIdQueryHandler(IUserRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<UserProfileDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(request.UsuarioId);
        if (usuario == null) return null;

        return new UserProfileDto(
            Id: usuario.Id,
            Nome: usuario.Nome,
            Email: usuario.Email.Value,
            Role: usuario.Papel.ToString()
        );
    }
}