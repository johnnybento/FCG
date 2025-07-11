using Bogus;
using FGC.Core.Sale.Entities;
using FGC.Core.Users.Entities;

namespace FCG.Core.Tests.Sales.TestData;

public class JogoAdquiridoTestData
{
    private readonly Faker _faker;

    public JogoAdquiridoTestData()
    {
        _faker = new Faker("pt_BR");
    }

    /// <summary>
    /// Gera um JogoAdquirido válido para o usuário informado
    /// </summary>
    public JogoAdquirido GerarJogoAdquiridoValido(Usuario usuario)
    {
        var jogoId = Guid.NewGuid();
        var preco = _faker.Random.Decimal(1m, 300m);
        return new JogoAdquirido(usuario.Id, jogoId, preco, usuario);
    }
}