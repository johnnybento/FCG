

using Bogus;
using FGC.Application.Sale.Commands.ComprarJogo;

namespace FCG.Application.Tests.Sale.TestData;

public class ComprarJogoCommandTestData
{
    private readonly Faker _faker;
    public ComprarJogoCommand Request { get; }

    public ComprarJogoCommandTestData()
    {
        _faker = new Faker("pt_BR");
        Request = new ComprarJogoCommand(
            UsuarioId: Guid.NewGuid(),
            JogoId: Guid.NewGuid()
        );
    }
}