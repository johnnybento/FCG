using FCG.Core.Tests.Catalog.TestData;
using FGC.Core.Catalog.Entities;
using FGC.Core.Exceptions;
using FluentAssertions;

namespace FCG.Core.Tests.Catalog;

public class PromocaoTests
{
    private readonly PromocaoTestData _fixture;

    public PromocaoTests() => _fixture = new PromocaoTestData();

    [Fact]
    public void Constructor_ComDadosValidos_DeveCriarPromocao()
    {
        // Arrange
        var (jogoId, desconto, inicio, termino) = _fixture.GerarDadosValidos();

        // Act
        var promo = new Promocao(jogoId, desconto, inicio, termino);

        // Assert
        promo.Id.Should().NotBe(Guid.Empty);
        promo.JogoId.Should().Be(jogoId);
        promo.Desconto.Should().Be(desconto);
        promo.Inicio.Should().Be(inicio);
        promo.Termino.Should().Be(termino);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void Constructor_DescontoMuitoBaixo_DeveLancarDomainException(decimal descontoInvalido)
    {
        // Arrange
        var hoje = DateTime.UtcNow.Date;
        var inicio = hoje.AddDays(-1);
        var termino = hoje.AddDays(1);

        // Act
        Action act = () => new Promocao(_fixture.JogoId, descontoInvalido, inicio, termino);

        // Assert
        act.Should()
           .Throw<DomainException>()
           .WithMessage("Desconto deve ser entre 1 e 100%.");
    }

    [Fact]
    public void Constructor_DescontoMuitoAlto_DeveLancarDomainException()
    {
        // Arrange
        var desconto = _fixture.GerarDescontoInvalidoAlto();
        var hoje = DateTime.UtcNow.Date;
        var inicio = hoje.AddDays(-1);
        var termino = hoje.AddDays(1);

        // Act
        Action act = () => new Promocao(_fixture.JogoId, desconto, inicio, termino);

        // Assert
        act.Should()
           .Throw<DomainException>()
           .WithMessage("Desconto deve ser entre 1 e 100%.");
    }

    [Fact]
    public void Constructor_InicioIgualTermino_DeveLancarDomainException()
    {
        // Arrange
        var desconto = _fixture.GerarDadosValidos().desconto;
        var (inicio, termino) = _fixture.GerarPeriodoInvalidoIgual();

        // Act
        Action act = () => new Promocao(_fixture.JogoId, desconto, inicio, termino);

        // Assert
        act.Should()
           .Throw<DomainException>()
           .WithMessage("Período de promoção inválido.");
    }

    [Fact]
    public void Constructor_TerminoPassado_DeveLancarDomainException()
    {
        // Arrange
        var desconto = _fixture.GerarDadosValidos().desconto;
        var (inicio, termino) = _fixture.GerarPeriodoTerminoPassado();

        // Act
        Action act = () => new Promocao(_fixture.JogoId, desconto, inicio, termino);

        // Assert
        act.Should()
           .Throw<DomainException>()
           .WithMessage("Período de promoção inválido.");
    }

    [Fact]
    public void EstaAtiva_AgoraDentroDoPeriodo_RetornaTrue()
    {
        // Arrange
        var (jogoId, desconto, inicio, termino) = _fixture.GerarDadosValidos();
        var promo = new Promocao(jogoId, desconto, inicio, termino);
        var agora = inicio.AddSeconds((termino - inicio).TotalSeconds / 2);

        // Act
        var ativa = promo.EstaAtiva(agora);

        // Assert
        ativa.Should().BeTrue();
    }

    [Theory]
    [InlineData(-2)] 
    [InlineData(2)] 
    public void EstaAtiva_AgoraForaDoPeriodo_RetornaFalse(int diasOffset)
    {
        // Arrange
        var (jogoId, desconto, inicio, termino) = _fixture.GerarDadosValidos();
        var promo = new Promocao(jogoId, desconto, inicio, termino);
        var agora = (diasOffset < 0
            ? inicio.AddDays(diasOffset)
            : termino.AddDays(diasOffset));

        // Act
        var ativa = promo.EstaAtiva(agora);

        // Assert
        ativa.Should().BeFalse();
    }
}