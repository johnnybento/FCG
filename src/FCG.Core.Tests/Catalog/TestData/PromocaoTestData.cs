
using Bogus;

namespace FCG.Core.Tests.Catalog.TestData;

public class PromocaoTestData
{
    private readonly Faker _faker;
    public Guid JogoId { get; }

    public PromocaoTestData()
    {
        _faker = new Faker("pt_BR");
        JogoId = Guid.NewGuid();
    }

    /// <summary>
    /// Gera dados válidos: desconto 1–100%, período com início antes de término
    /// e término >= hoje.
    /// </summary>
    public (Guid jogoId, decimal desconto, DateTime inicio, DateTime termino) GerarDadosValidos()
    {
        var desconto = _faker.Random.Decimal(1m, 100m);
        var hoje = DateTime.UtcNow.Date;
        var inicio = hoje.AddDays(-_faker.Random.Int(1, 10));
        var termino = hoje.AddDays(_faker.Random.Int(1, 10));
        return (JogoId, desconto, inicio, termino);
    }

    /// <summary>
    /// Desconto ≤ 0 ou > 100
    /// </summary>
    public decimal GerarDescontoInvalidoBaixo() => _faker.Random.Decimal(-50m, 0m);
    public decimal GerarDescontoInvalidoAlto() => _faker.Random.Decimal(101m, 200m);

    /// <summary>
    /// Início ≥ término
    /// </summary>
    public (DateTime inicio, DateTime termino) GerarPeriodoInvalidoIgual()
    {
        var dia = DateTime.UtcNow.Date.AddDays(_faker.Random.Int(1, 5));
        return (dia, dia);
    }

    /// <summary>
    /// Término < hoje
    /// </summary>
    public (DateTime inicio, DateTime termino) GerarPeriodoTerminoPassado()
    {
        var hoje = DateTime.UtcNow.Date;
        var termino = hoje.AddDays(-_faker.Random.Int(1, 5));
        var inicio = termino.AddDays(-_faker.Random.Int(1, 5));
        return (inicio, termino);
    }
}
