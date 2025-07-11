using FCG.Application.Tests.Users.TestData;
using FGC.Application.Users.Queries.GetUserById;
using FGC.Core.Users.Entities;
using FGC.Core.Users.Repositories;
using FluentAssertions;
using Moq;

namespace FCG.Application.Tests.Users;

public class GetUserByIdQueryHandlerTests
{
    private readonly Mock<IUserRepository> _usuarioRepo;
    private readonly GetUserByIdQueryHandler _handler;
    private readonly GetUserByIdQueryTestData _fixture;

    public GetUserByIdQueryHandlerTests()
    {
        _usuarioRepo = new Mock<IUserRepository>();
        _handler = new GetUserByIdQueryHandler(_usuarioRepo.Object);
        _fixture = new GetUserByIdQueryTestData();
    }

    [Fact]
    public async Task Handle_UsuarioNaoEncontrado_DeveRetornarNull()
    {
        // Arrange
        _usuarioRepo
            .Setup(r => r.GetByIdAsync(_fixture.UsuarioId))
            .ReturnsAsync((Usuario?)null);

        // Act
        var result = await _handler.Handle(_fixture.Query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
        _usuarioRepo.Verify(r => r.GetByIdAsync(_fixture.UsuarioId), Times.Once);
    }

    [Fact]
    public async Task Handle_UsuarioExistente_DeveRetornarUserProfileDto()
    {
        // Arrange
        _usuarioRepo
            .Setup(r => r.GetByIdAsync(_fixture.UsuarioId))
            .ReturnsAsync(_fixture.DomainUser);

        // Act
        var result = await _handler.Handle(_fixture.Query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(_fixture.ExpectedDto);
        _usuarioRepo.Verify(r => r.GetByIdAsync(_fixture.UsuarioId), Times.Once);
    }
}
