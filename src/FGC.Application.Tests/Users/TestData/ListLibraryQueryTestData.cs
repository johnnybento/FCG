using Bogus;
using FCG.Core.Users.Enum;
using FGC.Application.Users.Queries.ListLibrary;
using FGC.Core.Sale.Entities;
using FGC.Core.Users.Entities;
using FGC.Core.Users.ValueObjects;

namespace FCG.Application.Tests.Users.TestData;
public class ListLibraryQueryTestData
{
    private readonly Faker _faker;

    public Guid UsuarioId { get; }
    public ListLibraryQuery Query { get; }
    public Usuario DomainUser { get; }
    public List<JogoAdquirido> Compras { get; }
    public List<LibraryItemDto> Dtos { get; }

    public ListLibraryQueryTestData()
    {
        _faker = new Faker("pt_BR");
        UsuarioId = Guid.NewGuid();
        Query = new ListLibraryQuery(UsuarioId);

        DomainUser = new Usuario(
            nome: _faker.Name.FullName(),
            email: EmailVo.Create(_faker.Internet.Email()),
            senhaHash: _faker.Random.AlphaNumeric(32),
            papel: _faker.PickRandom<UserRole>()
        );

        var count = _faker.Random.Int(1, 5);
        Compras = Enumerable.Range(0, count)
            .Select(_ =>
            {
                var gameId = Guid.NewGuid();
                var preco = _faker.Random.Decimal(1m, 500m);
                return new JogoAdquirido(UsuarioId, gameId, preco, DomainUser);
            })
            .ToList();

        Dtos = Compras
            .Select(c => new LibraryItemDto(
                JogoId: c.JogoId,
                PrecoPago: c.PrecoPago,
                DataHora: c.DataHora
            ))
            .ToList();
    }
}
