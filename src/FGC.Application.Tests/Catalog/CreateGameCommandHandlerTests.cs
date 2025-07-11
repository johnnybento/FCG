using AutoMapper;
using FCG.Application.Tests.Catalog.TestData;
using FGC.Application.Catalog.Commands.CreateGame;
using FGC.Core.Catalog.Entities;
using FGC.Core.Catalog.Repositories;
using FGC.Core.Exceptions;
using FluentAssertions;
using Moq;

namespace FCG.Application.Tests.Catalog;

public class CreateGameCommandHandlerTests
{
    private readonly Mock<IJogoRepository> _repo;
    private readonly Mock<IMapper> _mapper;
    private readonly CreateGameCommandHandler _handler;
    private readonly CreateGameCommandTestData _fixture;
    private Jogo _capturedJogo;

    public CreateGameCommandHandlerTests()
    {
        _repo = new Mock<IJogoRepository>();
        _mapper = new Mock<IMapper>();
        _handler = new CreateGameCommandHandler(_repo.Object, _mapper.Object);
        _fixture = new CreateGameCommandTestData();

    
        _repo
            .Setup(r => r.AddAsync(It.IsAny<Jogo>()))
            .Callback<Jogo>(j => _capturedJogo = j)
            .Returns(Task.CompletedTask);

   
        _mapper
            .Setup(m => m.Map<CreateGameResponseDto>(It.IsAny<Jogo>()))
            .Returns<Jogo>(j => new CreateGameResponseDto(
                Id: j.Id,
                Titulo: j.Titulo,
                Descricao: j.Descricao,
                Preco: j.Preco
            ));
    }

    [Fact]
    public async Task Handle_TituloNovo_DeveChamarRepositorioEMapper()
    {
        // Arrange
        var cmd = _fixture.Request;
        _repo.Setup(r => r.TituloExistsAsync(cmd.Titulo))
             .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(cmd, CancellationToken.None);

        // Assert
 
        _repo.Verify(r => r.TituloExistsAsync(cmd.Titulo), Times.Once);
        _repo.Verify(r => r.AddAsync(It.IsAny<Jogo>()), Times.Once);

     
        _mapper.Verify(m => m.Map<CreateGameResponseDto>(_capturedJogo), Times.Once);

 
        result.Should().NotBeNull();
        result.Id.Should().Be(_capturedJogo.Id);
        result.Titulo.Should().Be(cmd.Titulo);
        result.Descricao.Should().Be(cmd.Descricao);
        result.Preco.Should().Be(cmd.Preco);
    }

    [Fact]
    public async Task Handle_TituloExistente_DeveLancarDomainException()
    {
        // Arrange
        var cmd = _fixture.Request;
        _repo.Setup(r => r.TituloExistsAsync(cmd.Titulo))
             .ReturnsAsync(true);

        // Act
        Func<Task> act = () => _handler.Handle(cmd, CancellationToken.None);

        // Assert
        await act
            .Should().ThrowAsync<DomainException>()
            .WithMessage("Já existe um jogo com este título.");


        _repo.Verify(r => r.AddAsync(It.IsAny<Jogo>()), Times.Never);
        _mapper.Verify(m => m.Map<CreateGameResponseDto>(It.IsAny<Jogo>()), Times.Never);
    }
}
