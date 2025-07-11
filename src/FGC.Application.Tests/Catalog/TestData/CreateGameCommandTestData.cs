using Bogus;
using FGC.Application.Catalog.Commands.CreateGame;

namespace FCG.Application.Tests.Catalog.TestData;

public class CreateGameCommandTestData
{
    private readonly Faker _faker;
    public CreateGameCommand Request { get; }

    public CreateGameCommandTestData()
    {
        _faker = new Faker("pt_BR");

        var titulo = _faker.Lorem.Sentence(2).TrimEnd('.');
        var descricao = _faker.Lorem.Paragraph();
        var preco = _faker.Random.Decimal(0m, 1000m);

        Request = new CreateGameCommand(
            Titulo: titulo,
            Descricao: descricao,
            Preco: preco
        );
    }
}