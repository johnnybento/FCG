using Bogus;
using FCG.Core.Users.Enum;
using FGC.Application.Auth.Commands.Login;
using FGC.Core.Users.Entities;
using FGC.Core.Users.ValueObjects;

namespace FCG.Application.Tests.Auth.TestData;

public class LoginCommandTestData
{
    private readonly Faker _faker;

    public string Email { get; }
    public string Senha { get; }
    public LoginCommand Request { get; }

    public LoginCommandTestData()
    {
        _faker = new Faker("pt_BR");
        Email = _faker.Internet.Email();
        Senha = _faker.Internet.Password();
        Request = new LoginCommand(Email, Senha);
    }

    /// <summary>
    /// Gera um usuário cuja propriedade SenhaHash
    /// será ignorada pelo stub do IPasswordHasher.Verify().
    /// </summary>
    public Usuario GetUsuario()
    {
        var nome = _faker.Name.FullName();
        var emailVo =  EmailVo.Create(Email);
        var senhaHash = _faker.Random.AlphaNumeric(32);
        var papel = _faker.PickRandom<UserRole>();
        return new Usuario(nome, emailVo, senhaHash, papel);
    }
}