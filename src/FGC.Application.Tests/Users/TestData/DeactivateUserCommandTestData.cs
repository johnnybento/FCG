using Bogus;
using FGC.Application.Users.Commands.DeactiveUser;

namespace FCG.Application.Tests.Users.TestData;
public class DeactivateUserCommandTestData
{
    private readonly Faker _faker;
    public DeactivateUserCommand Request { get; }

    public DeactivateUserCommandTestData()
    {
        _faker = new Faker("pt_BR");
        Request = new DeactivateUserCommand(
            UsuarioId: Guid.NewGuid()
        );
    }
}