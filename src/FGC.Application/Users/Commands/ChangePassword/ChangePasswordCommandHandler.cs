using FGC.Application.Common.Ports;
using FGC.Core.Exceptions;
using FGC.Core.Users.Repositories;
using FGC.Core.Users.ValueObjects;
using MediatR;

namespace FGC.Application.Users.Commands.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand>
{
    private readonly IUserRepository _usuarioRepository;
    private readonly IPasswordHasher _passwordHasher;

    public ChangePasswordCommandHandler(
        IUserRepository usuarioRepository,
        IPasswordHasher passwordHasher)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(request.UsuarioId);
        if (usuario == null)
            throw new DomainException("Usuário não encontrado.");

        if (!_passwordHasher.Verify( request.SenhaAtual, usuario.SenhaHash))
            throw new DomainException("Senha atual incorreta.");

        SenhaVo.Create(request.NovaSenha);
               
        var novaHash = _passwordHasher.Hash(request.NovaSenha);
        usuario.AlterarSenha(novaHash);

        await _usuarioRepository.UpdateAsync(usuario);
        return Unit.Value;
    }

    Task IRequestHandler<ChangePasswordCommand>.Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        return Handle(request, cancellationToken);
    }
}