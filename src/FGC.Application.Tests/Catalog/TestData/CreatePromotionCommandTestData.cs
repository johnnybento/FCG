using Bogus;
using FGC.Application.Catalog.Commands.CreatePromotion;

namespace FCG.Application.Tests.Catalog.TestData;

public class CreatePromotionCommandTestData
{
    private readonly Faker _faker;
    public CreatePromotionCommand Request { get; }

    public CreatePromotionCommandTestData()
    {
        _faker = new Faker("pt_BR");

        var jogoId = Guid.NewGuid();
        var desconto = _faker.Random.Decimal(1m, 100m);
        var hoje = DateTime.UtcNow.Date;
        var inicio = hoje.AddDays(-_faker.Random.Int(1, 5));
        var termino = hoje.AddDays(_faker.Random.Int(1, 5));

        Request = new CreatePromotionCommand(
            JogoId: jogoId,
            Desconto: desconto,
            Inicio: inicio,
            Termino: termino
        );
    }
}