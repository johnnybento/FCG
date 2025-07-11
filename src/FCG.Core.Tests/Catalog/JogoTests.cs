
using FCG.Core.Tests.Catalog.TestData;
using FGC.Core.Catalog.Entities;
using FGC.Core.Exceptions;
using FluentAssertions;

namespace FCG.Core.Tests.Catalog;

public class JogoTests
{
    private readonly JogoTestData _fixture;

    public JogoTests() => _fixture = new JogoTestData();

    [Fact]
    public void Constructor_ComDadosValidos_DeveCriarJogo()
    {
        // Arrange
        var (titulo, descricao, preco) = _fixture.GerarDadosValidos();

        // Act
        var jogo = new Jogo(titulo, descricao, preco);

        // Assert
        jogo.Id.Should().NotBe(Guid.Empty);
        jogo.Titulo.Should().Be(titulo.Trim());
        jogo.Descricao.Should().Be(descricao.Trim());
        jogo.Preco.Should().Be(preco);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_TituloInvalido_DeveLancarDomainException(string tituloInvalido)
    {
        // Arrange
        var (_, descricao, preco) = _fixture.GerarDadosValidos();

        // Act
        Action act = () => new Jogo(tituloInvalido!, descricao, preco);

        // Assert
        act.Should()
           .Throw<DomainException>()
           .WithMessage("Título do jogo não pode ser vazio.");
    }

    [Fact]
    public void Constructor_PrecoNegativo_DeveLancarDomainException()
    {
        // Arrange
        var (titulo, descricao, _) = _fixture.GerarDadosValidos();
        decimal precoNegativo = -0.01m;

        // Act
        Action act = () => new Jogo(titulo, descricao, precoNegativo);

        // Assert
        act.Should()
           .Throw<DomainException>()
           .WithMessage("Preço não pode ser negativo.");
    }

    [Fact]
    public void AtualizarDados_ComDadosValidos_DeveAtualizarPropriedades()
    {
        // Arrange
        var (titulo, descricao, preco) = _fixture.GerarDadosValidos();
        var jogo = new Jogo(titulo, descricao, preco);

        var (novoTitulo, novaDescricao, novoPreco) = _fixture.GerarDadosValidos();

        // Act
        jogo.AtualizarDados(novoTitulo, novaDescricao, novoPreco);

        // Assert
        jogo.Titulo.Should().Be(novoTitulo);
        jogo.Descricao.Should().Be(novaDescricao);
        jogo.Preco.Should().Be(novoPreco);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void AtualizarDados_NovoTituloInvalido_DeveLancarDomainException(string novoTitulo)
    {
        // Arrange
        var (titulo, descricao, preco) = _fixture.GerarDadosValidos();
        var jogo = new Jogo(titulo, descricao, preco);
        var (_, novaDescricao, novoPreco) = _fixture.GerarDadosValidos();

        // Act
        Action act = () => jogo.AtualizarDados(novoTitulo!, novaDescricao, novoPreco);

        // Assert
        act.Should()
           .Throw<DomainException>()
           .WithMessage($"Título é obrigatório. Título{nameof(novoTitulo)}");
    }

    [Fact]
    public void AtualizarDados_PrecoNegativo_DeveLancarDomainException()
    {
        // Arrange
        var (titulo, descricao, preco) = _fixture.GerarDadosValidos();
        var jogo = new Jogo(titulo, descricao, preco);
        var (novoTitulo, novaDescricao, _) = _fixture.GerarDadosValidos();
        decimal precoNegativo = -5m;

        // Act
        Action act = () => jogo.AtualizarDados(novoTitulo, novaDescricao, precoNegativo);

        // Assert
        act.Should()
           .Throw<DomainException>()
           .WithMessage($"Preço não pode ser negativo");
    }
}