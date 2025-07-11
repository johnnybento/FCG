using AutoMapper;
using FCG.Application.Tests.Catalog.TestData;
using FGC.Application.Catalog.Queries.ListGames;
using FGC.Core.Catalog.Repositories;
using FluentAssertions;
using Moq;

namespace FCG.Application.Tests.Catalog;

public class ListGamesQueryHandlerTests
{
    private readonly Mock<IJogoRepository> _repo;
    private readonly Mock<IMapper> _mapper;
    private readonly ListGamesQueryHandler _handler;
    private readonly ListGamesQueryTestData _fixture;

    public ListGamesQueryHandlerTests()
    {
        _repo = new Mock<IJogoRepository>();
        _mapper = new Mock<IMapper>();
        _handler = new ListGamesQueryHandler(_repo.Object, _mapper.Object);
        _fixture = new ListGamesQueryTestData();


        _repo
            .Setup(r => r.ListAsync())
            .ReturnsAsync(_fixture.Jogos);


        _mapper
            .Setup(m => m.Map<List<GameDto>>(_fixture.Jogos))
            .Returns(_fixture.Dtos);
    }

    [Fact]
    public async Task Handle_DeveRetornarListaDeGameDto()
    {
        // Act
        var result = await _handler.Handle(_fixture.Query, CancellationToken.None);

        // Assert

        _repo.Verify(r => r.ListAsync(), Times.Once);

      
        _mapper.Verify(m => m.Map<List<GameDto>>(_fixture.Jogos), Times.Once);


        result.Should().BeEquivalentTo(_fixture.Dtos);
    }
}