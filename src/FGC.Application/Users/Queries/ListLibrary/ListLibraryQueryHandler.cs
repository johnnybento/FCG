using AutoMapper;
using FGC.Core.Exceptions;
using FGC.Core.Sale.Repositories;
using FGC.Core.Users.Repositories;
using MediatR;

namespace FGC.Application.Users.Queries.ListLibrary;

public class ListLibraryQueryHandler
    : IRequestHandler<ListLibraryQuery, List<LibraryItemDto>>
{
    private readonly IUserRepository _usuarioRepository;
    private readonly IJogoAdquiridoRepository _compraRepository;
    private readonly IMapper _mapper;

    public ListLibraryQueryHandler(
        IUserRepository usuarioRepository,
        IJogoAdquiridoRepository compraRepository,
        IMapper mapper)
    {
        _usuarioRepository = usuarioRepository;
        _compraRepository = compraRepository;
        _mapper = mapper;
    }

    public async Task<List<LibraryItemDto>> Handle(
        ListLibraryQuery request,
        CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(request.UsuarioId);
        if (usuario == null)
            throw new DomainException("Usuário não encontrado.");

        var compras = await _compraRepository.GetByUsuarioIdAsync(request.UsuarioId);
        return compras
            .Select(c => _mapper.Map<LibraryItemDto>(c))
            .ToList();
    }
}