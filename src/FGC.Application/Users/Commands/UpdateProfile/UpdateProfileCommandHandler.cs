using AutoMapper;
using FGC.Core.Exceptions;
using FGC.Core.Users.Repositories;
using FGC.Core.Users.ValueObjects;
using MediatR;

namespace FGC.Application.Users.Commands.UpdateProfile;

public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, UpdateProfileResponseDto>
{
    private readonly IUserRepository _usuarioRepository;
    private readonly IMapper _mapper;

    public UpdateProfileCommandHandler(
        IUserRepository usuarioRepository,
        IMapper mapper)
    {
        _usuarioRepository = usuarioRepository;
        _mapper = mapper;
    }

    public async Task<UpdateProfileResponseDto> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(request.UsuarioId);
        if (usuario == null)
            throw new DomainException("Usuário não encontrado.");

        var emailVo = EmailVo.Create(request.Email);


        usuario.AtualizarPerfil(request.Nome, emailVo);
        await _usuarioRepository.UpdateAsync(usuario);

        return _mapper.Map<UpdateProfileResponseDto>(usuario);
    }
}
