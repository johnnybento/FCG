using AutoMapper;
using FCG.Core.Users.Enum;
using FGC.Application.Common.Ports;
using FGC.Core.Common.Events;
using FGC.Core.Exceptions;
using FGC.Core.Users.Entities;
using FGC.Core.Users.Events;
using FGC.Core.Users.Repositories;
using FGC.Core.Users.ValueObjects;
using MediatR;

namespace FGC.Application.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserResponseDto>
{
    private readonly IUserRepository _usuarioRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(
        IUserRepository usuarioRepository,
        IPasswordHasher passwordHasher,
        IMapper mapper)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
    }

    public async Task<CreateUserResponseDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {

        if (await _usuarioRepository.EmailExistsAsync(request.Email))
            throw new DomainException("Email já cadastrado.");


        var emailVo = EmailVo.Create(request.Email);
        var senhaVo = SenhaVo.Create(request.Senha);
        var senhaHash = _passwordHasher.Hash(request.Senha);


        var usuario = new Usuario(
            request.Nome,
            emailVo,
            senhaHash,
            UserRole.Usuario
        );

        DomainEvents.Raise(new UsuarioCadastradoEvent(usuario));

        await _usuarioRepository.AddAsync(usuario);


        var response = _mapper.Map<CreateUserResponseDto>(usuario);

        DomainEvents.Raise(new UsuarioCadastradoEvent(usuario));
        return response;
    }
}
