using Bogus;
using FCG.Core.Users.Enum;
using FGC.Core.Users.Entities;
using FGC.Core.Users.ValueObjects;

namespace FGC.Core.Tests.Users.TestData;

public class UsuarioTestData
{
    private readonly Faker _faker;

    public UsuarioTestData()
    {
        _faker = new Faker("pt_BR");
    }

    /// <summary>
    /// Gera uma instância de Usuario válida
    /// </summary>
    public Usuario GerarUsuarioValido()
    {
        var nome = _faker.Name.FullName();
        var emailVo =  EmailVo.Create(_faker.Internet.Email());
        var senhaHash = _faker.Random.AlphaNumeric(32);
        var papel = _faker.PickRandom<UserRole>();

        return new Usuario(nome, emailVo, senhaHash, papel);
    }

    /// <summary>
    /// Gera novos dados para Update de perfil
    /// </summary>
    public (string NovoNome, EmailVo NovoEmail) GerarPerfilUpdateValido()
    {
        var nome = _faker.Name.FullName();
        var email =  EmailVo.Create(_faker.Internet.Email());
        return (nome, email);
    }

    ///<summary>
    ///Gera uma senhaHash fake válida
    ///</summary>
    public string GerarSenhaHashValida()
        => _faker.Random.AlphaNumeric(32);
}