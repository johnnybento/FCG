using FCG.Application.Tests.Auth.TestData;
using FGC.Application.Auth.Commands.Login;
using FGC.Application.Common.Ports;
using FGC.Core.Exceptions;
using FGC.Core.Users.Entities;
using FGC.Core.Users.Repositories;
using FluentAssertions;
using Moq;

namespace FCG.Application.Tests.Auth;

public class LoginCommandHandlerTests
{
    private readonly Mock<IUserRepository> _usuarioRepo;
    private readonly Mock<IPasswordHasher> _passwordHasher;
    private readonly Mock<IJwtService> _jwtService;
    private readonly LoginCommandHandler _handler;
    private readonly LoginCommandTestData _fixture;

    public LoginCommandHandlerTests()
    {
        _usuarioRepo = new Mock<IUserRepository>();
        _passwordHasher = new Mock<IPasswordHasher>();
        _jwtService = new Mock<IJwtService>();
        _handler = new LoginCommandHandler(
            _usuarioRepo.Object,
            _passwordHasher.Object,
            _jwtService.Object);
        _fixture = new LoginCommandTestData();
    }

    [Fact]
    public async Task Handle_CredenciaisValidas_DeveRetornarDtoComToken()
    {
        // Arrange
        var cmd = _fixture.Request;
        var usuario = _fixture.GetUsuario();
        var token = Guid.NewGuid().ToString();

        _usuarioRepo
            .Setup(r => r.GetByEmailAsync(cmd.Email))
            .ReturnsAsync(usuario);

        _passwordHasher
            .Setup(h => h.Verify(cmd.Senha, usuario.SenhaHash))
            .Returns(true);

        _jwtService
            .Setup(j => j.GenerateToken(usuario))
            .Returns(token);

        // Act
        var result = await _handler.Handle(cmd, CancellationToken.None);

        // Assert
        result.Token.Should().Be(token);
        result.UsuarioId.Should().Be(usuario.Id);
        result.Email.Should().Be(usuario.Email.Value);
        result.Role.Should().Be(usuario.Papel.ToString());
    }

    [Fact]
    public async Task Handle_UsuarioNaoEncontrado_DeveLancarDomainException()
    {
        // Arrange
        _usuarioRepo
            .Setup(r => r.GetByEmailAsync(_fixture.Email))
            .ReturnsAsync((Usuario?)null);

        // Act
        Func<Task> act = () => _handler.Handle(_fixture.Request, CancellationToken.None);

        // Assert
        await act.Should()
                 .ThrowAsync<DomainException>()
                 .WithMessage("Credenciais inválidas.");
    }

    [Fact]
    public async Task Handle_SenhaIncorreta_DeveLancarDomainException()
    {
        // Arrange
        var usuario = _fixture.GetUsuario();

        _usuarioRepo
            .Setup(r => r.GetByEmailAsync(_fixture.Email))
            .ReturnsAsync(usuario);

        _passwordHasher
            .Setup(h => h.Verify(_fixture.Senha, usuario.SenhaHash))
            .Returns(false);

        // Act
        Func<Task> act = () => _handler.Handle(_fixture.Request, CancellationToken.None);

        // Assert
        await act.Should()
                 .ThrowAsync<DomainException>()
                 .WithMessage("Credenciais inválidas.");
    }
}