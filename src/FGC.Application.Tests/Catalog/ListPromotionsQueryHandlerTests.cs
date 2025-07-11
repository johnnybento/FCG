using AutoMapper;
using FCG.Application.Tests.Catalog.TestData;
using FGC.Application.Catalog.Queries.ListPromotions;
using FGC.Core.Catalog.Repositories;
using FluentAssertions;
using Moq;

namespace FCG.Application.Tests.Catalog;

public class ListPromotionsQueryHandlerTests
{
    private readonly Mock<IPromocaoRepository> _promoRepo;
    private readonly Mock<IMapper> _mapper;
    private readonly ListPromotionsQueryHandler _handler;
    private readonly ListPromotionsQueryTestData _fixture;

    public ListPromotionsQueryHandlerTests()
    {
        _promoRepo = new Mock<IPromocaoRepository>();
        _mapper = new Mock<IMapper>();
        _handler = new ListPromotionsQueryHandler(_promoRepo.Object, _mapper.Object);
        _fixture = new ListPromotionsQueryTestData();

       
        _promoRepo
            .Setup(r => r.GetActiveByJogoIdAsync(
                _fixture.Query.JogoId,
                It.IsAny<DateTime>()))
            .ReturnsAsync(_fixture.Promocoes);

   
        _mapper
            .Setup(m => m.Map<List<PromotionDto>>(_fixture.Promocoes))
            .Returns(_fixture.Dtos);
    }

    [Fact]
    public async Task Handle_DeveRetornarListaDePromotionDto()
    {
        // Act
        var result = await _handler.Handle(_fixture.Query, CancellationToken.None);

        // Assert

        
        _promoRepo.Verify(r =>
            r.GetActiveByJogoIdAsync(
                _fixture.Query.JogoId,
                It.IsAny<DateTime>()),
            Times.Once);

      
        _mapper.Verify(m =>
            m.Map<List<PromotionDto>>(_fixture.Promocoes),
            Times.Once);

   
        result.Should().BeEquivalentTo(_fixture.Dtos);
    }
}