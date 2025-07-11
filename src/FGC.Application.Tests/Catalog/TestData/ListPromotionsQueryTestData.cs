using Bogus;
using FGC.Application.Catalog.Queries.ListPromotions;
using FGC.Core.Catalog.Entities;

namespace FCG.Application.Tests.Catalog.TestData;

public class ListPromotionsQueryTestData
{
    private readonly Faker _faker;
    public ListPromotionsQuery Query { get; }
    public List<Promocao> Promocoes { get; }
    public List<PromotionDto> Dtos { get; }

    public ListPromotionsQueryTestData()
    {
        _faker = new Faker("pt_BR");

   
        var jogoId = Guid.NewGuid();
        Query = new ListPromotionsQuery(jogoId);

  
        var hoje = DateTime.UtcNow.Date;
        Promocoes = Enumerable.Range(0, 3)
            .Select(_ =>
            {
                var desconto = _faker.Random.Decimal(1m, 100m);
                var inicio = hoje.AddDays(-_faker.Random.Int(1, 5));
                var termino = hoje.AddDays(_faker.Random.Int(1, 5));
                return new Promocao(jogoId, desconto, inicio, termino);
            })
            .ToList();

        
        Dtos = Promocoes
            .Select(p => new PromotionDto(
                Id: p.Id,
                JogoId: p.JogoId,
                Desconto: p.Desconto,
                Inicio: p.Inicio,
                Termino: p.Termino
            ))
            .ToList();
    }
}
