using Bogus;
using FGC.Application.Catalog.Queries.ListGames;
using FGC.Core.Catalog.Entities;

namespace FCG.Application.Tests.Catalog.TestData;

public class ListGamesQueryTestData
{
    private readonly Faker _faker;

    public ListGamesQuery Query { get; }
    public List<Jogo> Jogos { get; }
    public List<GameDto> Dtos { get; }

    public ListGamesQueryTestData()
    {
        _faker = new Faker("pt_BR");

   
        Query = new ListGamesQuery();

 
        Jogos = Enumerable.Range(0, 3)
            .Select(_ => new Jogo(
                titulo: _faker.Lorem.Sentence(2).TrimEnd('.'),
                descricao: _faker.Lorem.Paragraph(),
                preco: _faker.Random.Decimal(0m, 500m)
            ))
            .ToList();

  
        Dtos = Jogos
            .Select(j => new GameDto(
                Id: j.Id,
                Titulo: j.Titulo,
                Descricao: j.Descricao,
                Preco: j.Preco
            ))
            .ToList();
    }
}