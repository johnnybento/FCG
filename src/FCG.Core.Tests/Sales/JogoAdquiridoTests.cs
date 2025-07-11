using FCG.Core.Tests.Sales.TestData;
using FGC.Core.Exceptions;
using FGC.Core.Tests.Users.TestData;
using FluentAssertions;

namespace FCG.Core.Tests.Sales;

public class JogoAdquiridoTests
{
    private readonly UsuarioTestData _usuarioFixture;
    private readonly JogoAdquiridoTestData _jogoFixture;

    public JogoAdquiridoTests()
    {
        _usuarioFixture = new UsuarioTestData();
        _jogoFixture = new JogoAdquiridoTestData();
    }

 
    [Fact]
    public void AdquirirJogo_ComJogoNovo_DeveAdicionarNaBiblioteca()
    {
        // Arrange
        var usuario = _usuarioFixture.GerarUsuarioValido();
        var compra = _jogoFixture.GerarJogoAdquiridoValido(usuario);

        // Act
        usuario.AdquirirJogo(compra);

        // Assert
        usuario.Biblioteca.Should().HaveCount(1);
        usuario.Biblioteca.Should().ContainSingle(x =>
            x.JogoId == compra.JogoId &&
            x.PrecoPago == compra.PrecoPago);
    }

    [Fact]
    public void AdquirirJogo_JogoRepetido_DeveLancarDomainException()
    {
        // Arrange
        var usuario = _usuarioFixture.GerarUsuarioValido();
        var compra = _jogoFixture.GerarJogoAdquiridoValido(usuario);
        usuario.AdquirirJogo(compra);

        // Act
        Action act = () => usuario.AdquirirJogo(compra);

        // Assert
        act.Should()
           .Throw<DomainException>()
           .WithMessage("Jogo já adquirido.");
    }
}