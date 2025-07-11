using Bogus;
using FCG.Core.Users.Enum;
using FGC.Application.Users.Queries.GetUserById;
using FGC.Core.Users.Entities;
using FGC.Core.Users.ValueObjects;

namespace FCG.Application.Tests.Users.TestData;

public class GetUserByIdQueryTestData
{
    private readonly Faker _faker;
    public Guid UsuarioId { get; }
    public GetUserByIdQuery Query { get; }
    public Usuario DomainUser { get; }
    public UserProfileDto ExpectedDto { get; }

    public GetUserByIdQueryTestData()
    {
        _faker = new Faker("pt_BR");
        UsuarioId = Guid.NewGuid();
        Query = new GetUserByIdQuery(UsuarioId);

        var nome = _faker.Name.FullName();
        var email = _faker.Internet.Email();
        var senhaHash = _faker.Random.AlphaNumeric(32);
        var papel = _faker.PickRandom<UserRole>();

        DomainUser = new Usuario(
            nome,
            EmailVo.Create(email),
            senhaHash,
            papel
        );

        ExpectedDto = new UserProfileDto(
            Id: DomainUser.Id,
            Nome: DomainUser.Nome,
            Email: DomainUser.Email.Value,
            Role: DomainUser.Papel.ToString()
        );
    }
}