using FCG.Application.Tests.Users.TestData;
using FCG.Core.Users.Enum;
using FGC.Application.Users.Commands.DeactiveUser;
using FGC.Core.Exceptions;
using FGC.Core.Users.Entities;
using FGC.Core.Users.Repositories;
using FGC.Core.Users.ValueObjects;
using FluentAssertions;
using MediatR;
using Moq;

namespace FCG.Application.Tests.Users;

public class DeactivateUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _usuarioRepo;
    private readonly DeactivateUserCommandHandler _handler;
    private readonly DeactivateUserCommandTestData _fixture;
    private Usuario _capturedUser;

    public DeactivateUserCommandHandlerTests()
    {
        _usuarioRepo = new Mock<IUserRepository>();
        _handler = new DeactivateUserCommandHandler(_usuarioRepo.Object);
        _fixture = new DeactivateUserCommandTestData();

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


        _usuarioRepo.Verify(r => r.UpdateAsync(It.IsAny<Usuario>()), Times.Never);
    }

    [Fact]
    public async Task Handle_UsuarioExistente_DeveDesativarEAtualizar()
    {
        // Arrange
        var usuario = new Usuario(
            nome: "Test User",
            email: EmailVo.Create("teste@fcg.com"),
            senhaHash: "hashdummy",
            papel: UserRole.Usuario
        );
        _usuarioRepo
            .Setup(r => r.GetByIdAsync(_fixture.Request.UsuarioId))
            .ReturnsAsync(usuario);

        // Act
        var result = await _handler.Handle(_fixture.Request, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        _usuarioRepo.Verify(r => r.UpdateAsync(It.Is<Usuario>(u => u.Id == usuario.Id)), Times.Once);
       _capturedUser.Should().NotBeNull();
        _capturedUser.Id.Should().Be(usuario.Id);
    }
}