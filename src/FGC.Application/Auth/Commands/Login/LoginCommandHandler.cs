using FGC.Application.Common.Ports;
using FGC.Core.Exceptions;
using FGC.Core.Users.Repositories;
using MediatR;

namespace FGC.Application.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResultDto>
{
    private readonly IUserRepository _usuarioRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;

    public LoginCommandHandler(
        IUserRepository usuarioRepository,
        IPasswordHasher passwordHasher,
        IJwtService jwtService)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public async Task<LoginResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.GetByEmailAsync(request.Email);
        if (usuario == null)
            throw new DomainException("Credenciais inválidas.");

        if (!_passwordHasher.Verify(request.Senha,usuario.SenhaHash))
            throw new DomainException("Credenciais inválidas.");

        var token = _jwtService.GenerateToken(usuario);

        return new LoginResultDto(
            Token: token,
            UsuarioId: usuario.Id,
            Email: usuario.Email.Value,
            Role: usuario.Papel.ToString()
        );
    }
}