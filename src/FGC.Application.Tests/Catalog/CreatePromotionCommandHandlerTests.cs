using AutoMapper;
using FCG.Application.Tests.Catalog.TestData;
using FGC.Application.Catalog.Commands.CreatePromotion;
using FGC.Core.Catalog.Entities;
using FGC.Core.Catalog.Repositories;
using FGC.Core.Exceptions;
using FluentAssertions;
using Moq;

namespace FCG.Application.Tests.Catalog;

public class CreatePromotionCommandHandlerTests
{
    private readonly Mock<IJogoRepository> _jogoRepo;
    private readonly Mock<IPromocaoRepository> _promoRepo;
    private readonly Mock<IMapper> _mapper;
    private readonly CreatePromotionCommandHandler _handler;
    private readonly CreatePromotionCommandTestData _fixture;
    private Promocao _capturedPromo;

    public CreatePromotionCommandHandlerTests()
    {
        _jogoRepo = new Mock<IJogoRepository>();
        _promoRepo = new Mock<IPromocaoRepository>();
        _mapper = new Mock<IMapper>();
        _handler = new CreatePromotionCommandHandler(
            _jogoRepo.Object,
            _promoRepo.Object,
            _mapper.Object
        );
        _fixture = new CreatePromotionCommandTestData();

        _promoRepo
            .Setup(r => r.AddAsync(It.IsAny<Promocao>()))
            .Callback<Promocao>(p => _capturedPromo = p)
            .Returns(Task.CompletedTask);


        _mapper
            .Setup(m => m.Map<CreatePromotionResponseDto>(It.IsAny<Promocao>()))
            .Returns<Promocao>(p => new CreatePromotionResponseDto(
                Id: p.Id,
                JogoId: p.JogoId,
                Desconto: p.Desconto,
                Inicio: p.Inicio,
                Termino: p.Termino
            ));
    }

    [Fact]
    public async Task Handle_JogoNaoExiste_DeveLancarDomainException()
    {
        // Arrange
        var cmd = _fixture.Request;
        _jogoRepo
            .Setup(r => r.GetByIdAsync(cmd.JogoId))
            .ReturnsAsync((Jogo?)null);

        // Act
        Func<Task> act = () => _handler.Handle(cmd, CancellationToken.None);

        // Assert
        await act.Should()
                 .ThrowAsync<DomainException>()
                 .WithMessage("Jogo não encontrado.");

        _promoRepo.Verify(r => r.AddAsync(It.IsAny<Promocao>()), Times.Never);
        _mapper.Verify(m => m.Map<CreatePromotionResponseDto>(It.IsAny<Promocao>()), Times.Never);
    }

    [Fact]
    public async Task Handle_DadosValidos_DeveAdicionarPromocaoERetornarDto()
    {
        // Arrange
        var cmd = _fixture.Request;
        var jogo = new Jogo("Título Teste", "Descrição Teste", 0m);
        _jogoRepo
            .Setup(r => r.GetByIdAsync(cmd.JogoId))
            .ReturnsAsync(jogo);

        // Act
        var result = await _handler.Handle(cmd, CancellationToken.None);

        // Assert
        _jogoRepo.Verify(r => r.GetByIdAsync(cmd.JogoId), Times.Once);
        _promoRepo.Verify(r => r.AddAsync(It.IsAny<Promocao>()), Times.Once);
        _mapper.Verify(m => m.Map<CreatePromotionResponseDto>(_capturedPromo), Times.Once);

        result.Should().NotBeNull();
        result.Id.Should().Be(_capturedPromo.Id);
        result.JogoId.Should().Be(cmd.JogoId);
        result.Desconto.Should().Be(cmd.Desconto);
        result.Inicio.Should().Be(cmd.Inicio);
        result.Termino.Should().Be(cmd.Termino);
    }
}