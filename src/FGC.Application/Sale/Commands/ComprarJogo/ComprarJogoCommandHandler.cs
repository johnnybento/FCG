using FGC.Core.Catalog.Repositories;
using FGC.Core.Exceptions;
using FGC.Core.Sale.Entities;
using FGC.Core.Sale.Repositories;
using FGC.Core.Users.Repositories;
using MediatR;

namespace FGC.Application.Sale.Commands.ComprarJogo;

public class ComprarJogoCommandHandler : IRequestHandler<ComprarJogoCommand, ComprarJogoResponseDto>
{
    private readonly IUserRepository _usuarioRepository;
    private readonly IJogoRepository _jogoRepository;
    private readonly IPromocaoRepository _promocaoRepository;
    private readonly IJogoAdquiridoRepository _compraRepository;

    public ComprarJogoCommandHandler(
        IUserRepository usuarioRepository,
        IJogoRepository jogoRepository,
        IPromocaoRepository promocaoRepository,
        IJogoAdquiridoRepository compraRepository)
    {
        _usuarioRepository = usuarioRepository;
        _jogoRepository = jogoRepository;
        _promocaoRepository = promocaoRepository;
        _compraRepository = compraRepository;
    }

    public async Task<ComprarJogoResponseDto> Handle(ComprarJogoCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(request.UsuarioId);
        if (usuario == null)
            throw new DomainException("Usuário não encontrado.");

        var jogo = await _jogoRepository.GetByIdAsync(request.JogoId);
        if (jogo == null)
            throw new DomainException("Jogo não encontrado.");

        if (await _compraRepository.ExistsAsync(request.UsuarioId, request.JogoId))
            throw new DomainException("Jogo já adquirido.");

        var now = DateTime.UtcNow;
        var promocoes = await _promocaoRepository.GetActiveByJogoIdAsync(request.JogoId, now);
        var desconto = promocoes.Any() ? promocoes.Max(p => p.Desconto) : 0m;
        var precoFinal = jogo.Preco * (1m - desconto);


        var compra = new JogoAdquirido(request.UsuarioId, request.JogoId, precoFinal, usuario);       

        await _compraRepository.AddAsync(compra);

        return new ComprarJogoResponseDto(compra.JogoId, compra.PrecoPago, compra.DataHora);
    }
}