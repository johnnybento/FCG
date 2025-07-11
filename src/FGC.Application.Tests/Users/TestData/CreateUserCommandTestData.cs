using Bogus;
using FGC.Application.Users.Commands.CreateUser;

namespace FCG.Application.Tests.Users.TestData;

public class CreateUserCommandTestData
{
    private readonly Faker _faker;

    public CreateUserCommand Request { get; }

    public CreateUserCommandTestData()
    {
        _faker = new Faker("pt_BR");

        Request = new CreateUserCommand(
            Nome: _faker.Name.FullName(),
            Email: _faker.Internet.Email(),
            Senha: _faker.Internet.Password(8, false, "\\w")
        );
    }
}