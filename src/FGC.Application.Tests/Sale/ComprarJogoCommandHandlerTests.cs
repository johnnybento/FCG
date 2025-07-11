using Bogus;
using FCG.Application.Tests.Sale.TestData;
using FCG.Core.Users.Enum;
using FGC.Application.Sale.Commands.ComprarJogo;
using FGC.Core.Catalog.Entities;
using FGC.Core.Catalog.Repositories;
using FGC.Core.Exceptions;
using FGC.Core.Sale.Entities;
using FGC.Core.Sale.Repositories;
using FGC.Core.Users.Entities;
using FGC.Core.Users.Repositories;
using FGC.Core.Users.ValueObjects;
using FluentAssertions;
using Moq;

namespace FCG.Application.Tests.Sale;

public class ComprarJogoCommandHandlerTests
{
    private readonly Mock<IUserRepository> _usuarioRepo;
    private readonly Mock<IJogoRepository> _jogoRepo;
    private readonly Mock<IPromocaoRepository> _promocaoRepo;
    private readonly Mock<IJogoAdquiridoRepository> _compraRepo;
    private readonly ComprarJogoCommandHandler _handler;
    private readonly ComprarJogoCommandTestData _fixture;
    private readonly Faker _faker;

    private JogoAdquirido _capturedCompra;

    public ComprarJogoCommandHandlerTests()
    {
        _usuarioRepo = new Mock<IUserRepository>();
        _jogoRepo = new Mock<IJogoRepository>();
        _promocaoRepo = new Mock<IPromocaoRepository>();
        _compraRepo = new Mock<IJogoAdquiridoRepository>();

        _handler = new ComprarJogoCommandHandler(
            _usuarioRepo.Object,
            _jogoRepo.Object,
            _promocaoRepo.Object,
            _compraRepo.Object
        );

        _fixture = new ComprarJogoCommandTestData();
        _faker = new Faker("pt_BR");

        _compraRepo
            .Setup(r => r.AddAsync(It.IsAny<JogoAdquirido>()))
            .Callback<JogoAdquirido>(c => _capturedCompra = c)
            .Returns(Task.CompletedTask);
    }

    [Fact]
    public async Task Handle_UsuarioNaoEncontrado_DeveLancarDomainException()
    {
        // Arrange
        _usuarioRepo
            .Setup(r => r.GetByIdAsync(_fixture.Request.UsuarioId))
            .ReturnsAsync((Usuario?)null);

        // Act
        Func<Task> act = () => _handler.Handle(_fixture.Request, CancellationToken.None);

        // Assert
        await act.Should()
                 .ThrowAsync<DomainException>()
                 .WithMessage("Usuário não encontrado.");

        _jogoRepo.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _compraRepo.Verify(r => r.ExistsAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
        _compraRepo.Verify(r => r.AddAsync(It.IsAny<JogoAdquirido>()), Times.Never);
    }

    [Fact]
    public async Task Handle_JogoNaoEncontrado_DeveLancarDomainException()
    {
        // Arrange
        var usuario = new Usuario(
            _faker.Name.FullName(),
             EmailVo.Create(_faker.Internet.Email()),
            _faker.Random.AlphaNumeric(32),
            _faker.PickRandom<UserRole>()
        );
        _usuarioRepo
            .Setup(r => r.GetByIdAsync(_fixture.Request.UsuarioId))
            .ReturnsAsync(usuario);
        _jogoRepo
            .Setup(r => r.GetByIdAsync(_fixture.Request.JogoId))
            .ReturnsAsync((Jogo?)null);

        // Act
        Func<Task> act = () => _handler.Handle(_fixture.Request, CancellationToken.None);

        // Assert
        await act.Should()
                 .ThrowAsync<DomainException>()
                 .WithMessage("Jogo não encontrado.");

        _compraRepo.Verify(r => r.ExistsAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
        _compraRepo.Verify(r => r.AddAsync(It.IsAny<JogoAdquirido>()), Times.Never);
    }

    [Fact]
    public async Task Handle_JogoJaAdquirido_DeveLancarDomainException()
    {
        // Arrange
        var usuario = new Usuario(
            _faker.Name.FullName(),
             EmailVo.Create(_faker.Internet.Email()),
            _faker.Random.AlphaNumeric(32),
            _faker.PickRandom<UserRole>()
        );
        var jogo = new Jogo(
            _faker.Lorem.Sentence(2).TrimEnd('.'),
            _faker.Lorem.Paragraph(),
            _faker.Random.Decimal(1m, 100m)
        );

        _usuarioRepo
            .Setup(r => r.GetByIdAsync(_fixture.Request.UsuarioId))
            .ReturnsAsync(usuario);
        _jogoRepo
            .Setup(r => r.GetByIdAsync(_fixture.Request.JogoId))
            .ReturnsAsync(jogo);
        _compraRepo
            .Setup(r => r.ExistsAsync(_fixture.Request.UsuarioId, _fixture.Request.JogoId))
            .ReturnsAsync(true);

        // Act
        Func<Task> act = () => _handler.Handle(_fixture.Request, CancellationToken.None);

        // Assert
        await act.Should()
                 .ThrowAsync<DomainException>()
                 .WithMessage("Jogo já adquirido.");

        _compraRepo.Verify(r => r.AddAsync(It.IsAny<JogoAdquirido>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ComprasSemPromocao_DeveRetornarResponseComPrecoInteiro()
    {
        // Arrange
        var usuario = new Usuario(
            _faker.Name.FullName(),
            EmailVo.Create(_faker.Internet.Email()),
            _faker.Random.AlphaNumeric(32),
            _faker.PickRandom<UserRole>()
        );
        var preco = _faker.Random.Decimal(1m, 200m);
        var jogo = new Jogo(
            _faker.Lorem.Sentence(2).TrimEnd('.'),
            _faker.Lorem.Paragraph(),
            preco
        );

        _usuarioRepo
            .Setup(r => r.GetByIdAsync(_fixture.Request.UsuarioId))
            .ReturnsAsync(usuario);
        _jogoRepo
            .Setup(r => r.GetByIdAsync(_fixture.Request.JogoId))
            .ReturnsAsync(jogo);
        _compraRepo
            .Setup(r => r.ExistsAsync(_fixture.Request.UsuarioId, _fixture.Request.JogoId))
            .ReturnsAsync(false);
        _promocaoRepo
            .Setup(r => r.GetActiveByJogoIdAsync(
                _fixture.Request.JogoId,
                It.IsAny<DateTime>()))
            .ReturnsAsync(new List<Promocao>());

        var now = DateTime.UtcNow;

        // Act
        var result = await _handler.Handle(_fixture.Request, CancellationToken.None);

        // Assert
        _compraRepo.Verify(r => r.AddAsync(It.IsAny<JogoAdquirido>()), Times.Once);
        result.JogoId.Should().Be(_fixture.Request.JogoId);
        result.PrecoPago.Should().Be(preco);
        result.DataHora.Should().BeCloseTo(now, TimeSpan.FromSeconds(1));
    }

    
}
