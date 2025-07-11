using FGC.Core.Exceptions;
using FGC.Core.Users.Repositories;
using MediatR;

namespace FGC.Application.Users.Commands.DeactiveUser;

public class DeactivateUserCommandHandler : IRequestHandler<DeactivateUserCommand>
{
    private readonly IUserRepository _usuarioRepository;

    public DeactivateUserCommandHandler(IUserRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<Unit> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(request.UsuarioId);
        if (usuario == null)
            throw new DomainException("Usuário não encontrado.");

        usuario.Desativar();
        await _usuarioRepository.RemoveAsync(usuario.Id);

        return Unit.Value;
    }

    Task IRequestHandler<DeactivateUserCommand>.Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
    {
        return Handle(request, cancellationToken);
    }
}
