using AutoMapper;
using FCG.Application.Tests.Users.TestData;
using FGC.Application.Users.Queries.ListLibrary;
using FGC.Core.Exceptions;
using FGC.Core.Sale.Entities;
using FGC.Core.Sale.Repositories;
using FGC.Core.Users.Entities;
using FGC.Core.Users.Repositories;
using FluentAssertions;
using Moq;

namespace FCG.Application.Tests.Users;

public class ListLibraryQueryHandlerTests
{
    private readonly Mock<IUserRepository> _usuarioRepo;
    private readonly Mock<IJogoAdquiridoRepository> _compraRepo;
    private readonly Mock<IMapper> _mapper;
    private readonly ListLibraryQueryHandler _handler;
    private readonly ListLibraryQueryTestData _fixture;

    public ListLibraryQueryHandlerTests()
    {
        _usuarioRepo = new Mock<IUserRepository>();
        _compraRepo = new Mock<IJogoAdquiridoRepository>();
        _mapper = new Mock<IMapper>();
        _handler = new ListLibraryQueryHandler(
            _usuarioRepo.Object,
            _compraRepo.Object,
            _mapper.Object
        );
        _fixture = new ListLibraryQueryTestData();

        
        _mapper
            .Setup(m => m.Map<LibraryItemDto>(It.IsAny<JogoAdquirido>()))
            .Returns<JogoAdquirido>(c =>
                _fixture.Dtos[_fixture.Compras.IndexOf(c)]
            );
    }

    [Fact]
    public async Task Handle_UsuarioNaoEncontrado_DeveLancarDomainException()
    {
        // Arrange
        _usuarioRepo
            .Setup(r => r.GetByIdAsync(_fixture.UsuarioId))
            .ReturnsAsync((Usuario?)null);

        // Act
        Func<Task> act = () => _handler.Handle(_fixture.Query, CancellationToken.None);

        // Assert
        await act.Should()
                 .ThrowAsync<DomainException>()
                 .WithMessage("Usuário não encontrado.");

        _compraRepo.Verify(r =>
            r.GetByUsuarioIdAsync(It.IsAny<Guid>()),
            Times.Never);
        _mapper.Verify(m =>
            m.Map<LibraryItemDto>(It.IsAny<JogoAdquirido>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_UsuarioExistente_DeveRetornarListaDeDtos()
    {
        // Arrange
        _usuarioRepo
            .Setup(r => r.GetByIdAsync(_fixture.UsuarioId))
            .ReturnsAsync(_fixture.DomainUser);

        _compraRepo
            .Setup(r => r.GetByUsuarioIdAsync(_fixture.UsuarioId))
            .ReturnsAsync(_fixture.Compras);

        // Act
        var result = await _handler.Handle(_fixture.Query, CancellationToken.None);

        // Assert
        _usuarioRepo.Verify(r =>
            r.GetByIdAsync(_fixture.UsuarioId), Times.Once);
        _compraRepo.Verify(r =>
            r.GetByUsuarioIdAsync(_fixture.UsuarioId), Times.Once);

        _mapper.Verify(m =>
            m.Map<LibraryItemDto>(It.IsAny<JogoAdquirido>()),
            Times.Exactly(_fixture.Compras.Count));
        result.Should().BeEquivalentTo(_fixture.Dtos);
    }
}
