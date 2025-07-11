using AutoMapper;
using FGC.Core.Catalog.Entities;
using FGC.Core.Catalog.Repositories;
using FGC.Core.Exceptions;
using MediatR;

namespace FGC.Application.Catalog.Commands.CreateGame;

public class CreateGameCommandHandler : IRequestHandler<CreateGameCommand, CreateGameResponseDto>
{
    private readonly IJogoRepository _jogoRepository;
    private readonly IMapper _mapper;

    public CreateGameCommandHandler(IJogoRepository jogoRepository, IMapper mapper)
    {
        _jogoRepository = jogoRepository;
        _mapper = mapper;
    }

    public async Task<CreateGameResponseDto> Handle(CreateGameCommand request, CancellationToken cancellationToken)
    {
        if (await _jogoRepository.TituloExistsAsync(request.Titulo))
            throw new DomainException("Já existe um jogo com este título.");

        var jogo = new Jogo(request.Titulo, request.Descricao, request.Preco);
        await _jogoRepository.AddAsync(jogo);

        return _mapper.Map<CreateGameResponseDto>(jogo);
    }
}