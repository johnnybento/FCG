using AutoMapper;
using FGC.Core.Catalog.Entities;
using FGC.Core.Catalog.Repositories;
using FGC.Core.Exceptions;
using MediatR;

namespace FGC.Application.Catalog.Commands.CreatePromotion;

public class CreatePromotionCommandHandler : IRequestHandler<CreatePromotionCommand, CreatePromotionResponseDto>
{
    private readonly IJogoRepository _jogoRepository;
    private readonly IPromocaoRepository _promocaoRepository;
    private readonly IMapper _mapper;

    public CreatePromotionCommandHandler(
        IJogoRepository jogoRepository,
        IPromocaoRepository promocaoRepository,
        IMapper mapper)
    {
        _jogoRepository = jogoRepository;
        _promocaoRepository = promocaoRepository;
        _mapper = mapper;
    }

    public async Task<CreatePromotionResponseDto> Handle(CreatePromotionCommand request, CancellationToken cancellationToken)
    {
        var jogo = await _jogoRepository.GetByIdAsync(request.JogoId);
        if (jogo == null)
            throw new DomainException("Jogo não encontrado.");

        var promocao = new Promocao(request.JogoId, request.Desconto, request.Inicio, request.Termino);
        await _promocaoRepository.AddAsync(promocao);

        return _mapper.Map<CreatePromotionResponseDto>(promocao);
    }
}