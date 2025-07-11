using Bogus;
using FGC.Application.Users.Commands.ChangePassword;

namespace FCG.Application.Tests.Users.TestData;

public class ChangePasswordCommandTestData
{
    private readonly Faker _faker;
    public ChangePasswordCommand Request { get; }

    public ChangePasswordCommandTestData()
    {
        _faker = new Faker("pt_BR");
        var senhaAtual = _faker.Internet.Password(8, false, "\\w");
        var novaSenha = _faker.Internet.Password(8, false, "\\w");
        Request = new ChangePasswordCommand(
            UsuarioId: Guid.NewGuid(),
            SenhaAtual: senhaAtual,
            NovaSenha: novaSenha
        );
    }
}