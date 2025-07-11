using Bogus;
using FGC.Application.Users.Commands.UpdateProfile;

namespace FCG.Application.Tests.Users.TestData;

public class UpdateProfileCommandTestData
{
    private readonly Faker _faker;
    public Guid UsuarioId { get; }
    public string NovoNome { get; }
    public string NovoEmail { get; }
    public UpdateProfileCommand Request { get; }

    public UpdateProfileCommandTestData()
    {
        _faker = new Faker("pt_BR");
        UsuarioId = Guid.NewGuid();
        NovoNome = _faker.Name.FullName();
        NovoEmail = _faker.Internet.Email();
        Request = new UpdateProfileCommand(
            UsuarioId: UsuarioId,
            Nome: NovoNome,
            Email: NovoEmail
        );
    }
}
