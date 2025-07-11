
using Bogus;

namespace FCG.Core.Tests.Catalog.TestData;

public class JogoTestData
{
    private readonly Faker _faker;

    public JogoTestData()
    {
        _faker = new Faker("pt_BR");
    }

    /// <summary>
    /// Gera um tupla com (titulo, descricao, preco) válidos.
    /// </summary>
    public (string Titulo, string Descricao, decimal Preco) GerarDadosValidos()
    {
        var titulo = _faker.Lorem.Sentence(3).TrimEnd('.');
        var descricao = _faker.Lorem.Paragraph();
        var preco = _faker.Random.Decimal(0m, 1000m);
        return (titulo, descricao, preco);
    }
}
