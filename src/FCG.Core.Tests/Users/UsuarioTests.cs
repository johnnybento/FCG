using FCG.Core.Users.Enum;
using FGC.Core.Exceptions;
using FGC.Core.Tests.Users.TestData;
using FGC.Core.Users.Entities;
using FGC.Core.Users.ValueObjects;
using FluentAssertions;

namespace FCG.Core.Tests.Users;

public class UsuarioTests
{
    private readonly UsuarioTestData _usuarioFixture;

    public UsuarioTests()
        => _usuarioFixture = new UsuarioTestData();

    [Fact]
    public void Constructor_ComDadosValidos_DeveCriarUsuario()
    {
        // Act
        var usuario = _usuarioFixture.GerarUsuarioValido();

        // AssertD
        usuario.Id.Should().NotBe(Guid.Empty);
        usuario.Nome.Should().NotBeNullOrWhiteSpace();
        usuario.Email.Value.Should().Contain("@");
        usuario.SenhaHash.Length.Should().Be(32);
        Enum.IsDefined(typeof(UserRole), usuario.Papel).Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_NomeInvalido_DeveLancarDomainException(string nomeInválido)
    {
        // Arrange
        var email =  EmailVo.Create("test@fcg.com");
        var senha = "hashqualquer";
        Action act = () => new Usuario(nomeInválido, email, senha, UserRole.Usuario);

        // Act & Assert
        act.Should()
           .Throw<DomainException>()
           .WithMessage("Nome não pode ser vazio ou nulo.");
    }

    [Fact]
    public void Constructor_EmailNull_DeveLancarDomainException()
    {
        // Arrange
        var nome = "Usuário Teste";
        var senha = "hashqualquer";
        Action act = () => new Usuario(nome, null!, senha, UserRole.Usuario);

        // Act & Assert
        act.Should()
           .Throw<DomainException>()
           .WithMessage("Email não pode ser vazio ou nulo");
    }

    [Fact]
    public void Constructor_SenhaHashNull_DeveLancarDomainException()
    {
        // Arrange
        var nome = "Usuário Teste";
        var email = EmailVo.Create("test@fcg.com");
        Action act = () => new Usuario(nome, email, null!, UserRole.Usuario);

        // Act & Assert
        act.Should()
           .Throw<DomainException>()
           .WithMessage("Hash da senha inválido.");
    }

    [Fact]
    public void AtualizarPerfil_ComDadosValidos_DeveAtualizarPropriedades()
    {
        // Arrange
        var usuario = _usuarioFixture.GerarUsuarioValido();
        var (novoNome, novoEmail) = _usuarioFixture.GerarPerfilUpdateValido();

        // Act
        usuario.AtualizarPerfil(novoNome, novoEmail);

        // Assert
        usuario.Nome.Should().Be(novoNome);
        usuario.Email.Should().Be(novoEmail);
    }

    [Fact]
    public void AtualizarPerfil_NomeInvalido_DeveLancarDomainException()
    {
        // Arrange
        var usuario = _usuarioFixture.GerarUsuarioValido();
        var email =  EmailVo.Create("test@fcg.com");
        Action act = () => usuario.AtualizarPerfil("   ", email);

        // Act & Assert
        act.Should()
           .Throw<DomainException>()
           .WithMessage("Nome não pode ser vazio ou nulo.");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void AlterarSenha_HashInvalido_DeveLancarDomainException(string hashInválido)
    {
        // Arrange
        var usuario = _usuarioFixture.GerarUsuarioValido();
        Action act = () => usuario.AlterarSenha(hashInválido!);

        // Act & Assert
        act.Should()
           .Throw<DomainException>()
           .WithMessage("Hash de senha inválido.");
    }

    [Fact]
    public void AlterarSenha_ComHashValido_DeveAtualizarSenhaHash()
    {
        // Arrange
        var usuario = _usuarioFixture.GerarUsuarioValido();
        var novaHash = _usuarioFixture.GerarSenhaHashValida();

        // Act
        usuario.AlterarSenha(novaHash);

        // Assert
        usuario.SenhaHash.Should().Be(novaHash);
    }

    [Fact]
    public void DefinirPapel_NovoPapel_DeveAtualizarPapel()
    {
        // Arrange
        var usuario = _usuarioFixture.GerarUsuarioValido();
        var novoPapel = UserRole.Administrador;

        // Act
        usuario.DefinirPapel(novoPapel);

        // Assert
        usuario.Papel.Should().Be(novoPapel);
    }

    [Fact]
    public void Biblioteca_Inicial_DeveEstarVazia()
    {
        // Arrange
        var usuario = _usuarioFixture.GerarUsuarioValido();

        // Act e Assert
        usuario.Biblioteca.Should().BeEmpty();
    }

}