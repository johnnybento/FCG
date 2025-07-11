using Bogus;
using FCG.Application.Tests.Users.TestData;
using FGC.Application.Common.Ports;
using FGC.Application.Users.Commands.ChangePassword;
using FGC.Core.Exceptions;
using FGC.Core.Users.Entities;
using FGC.Core.Users.Repositories;
using FGC.Core.Users.ValueObjects;
using FluentAssertions;
using MediatR;
using Moq;

namespace FCG.Application.Tests.Users;
public class ChangePasswordCommandHandlerTests
{
    private readonly Mock<IUserRepository> _usuarioRepo;
    private readonly Mock<IPasswordHasher> _passwordHasher;
    private readonly ChangePasswordCommandHandler _handler;
    private readonly ChangePasswordCommandTestData _fixture;
    private readonly Faker _faker;
    private Usuario _capturedUser;

    public ChangePasswordCommandHandlerTests()
    {
        _usuarioRepo = new Mock<IUserRepository>();
        _passwordHasher = new Mock<IPasswordHasher>();
        _handler = new ChangePasswordCommandHandler(
            _usuarioRepo.Object,
            _passwordHasher.Object);
        _fixture = new ChangePasswordCommandTestData();
        _faker = new Faker("pt_BR");

   
        _usuarioRepo
            .Setup(r => r.UpdateAsync(It.IsAny<Usuario>()))
            .Callback<Usuario>(u => _capturedUser = u)
            .Returns(Task.CompletedTask);
    }

    [Fact]
    public async Task Handle_UsuarioNaoEncontrado_DeveLancarDomainException()
    {
        // Arrange
        _usuarioRepo
            .Setup(r => r.GetByIdAsync(_fixture.Request.UsuarioId))
            .ReturnsAsync((Usuario?)null);

        // Act
        Func<Task> act = () => _handler.Handle(_fixture.Request, CancellationToken.None);

        // Assert
        await act.Should()
                 .ThrowAsync<DomainException>()
                 .WithMessage("Usuário não encontrado.");

        _passwordHasher.Verify(h => h.Verify(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        _usuarioRepo.Verify(r => r.UpdateAsync(It.IsAny<Usuario>()), Times.Never);
    }

    [Fact]
    public async Task Handle_SenhaAtualIncorreta_DeveLancarDomainException()
    {
        // Arrange
        var hashAntigo = _faker.Random.AlphaNumeric(32);
        var usuario = new Usuario(
            nome: _faker.Name.FullName(),
            email: EmailVo.Create(_faker.Internet.Email()),
            senhaHash: hashAntigo,
            papel: default);
        _usuarioRepo
            .Setup(r => r.GetByIdAsync(_fixture.Request.UsuarioId))
            .ReturnsAsync(usuario);
        _passwordHasher
            .Setup(h => h.Verify(hashAntigo, _fixture.Request.SenhaAtual))
            .Returns(false);

        // Act
        Func<Task> act = () => _handler.Handle(_fixture.Request, CancellationToken.None);

        // Assert
        await act.Should()
                 .ThrowAsync<DomainException>()
                 .WithMessage("Senha atual incorreta.");

        _passwordHasher.Verify(h => h.Verify(hashAntigo, _fixture.Request.SenhaAtual), Times.Once);
        _usuarioRepo.Verify(r => r.UpdateAsync(It.IsAny<Usuario>()), Times.Never);
    }

   
}