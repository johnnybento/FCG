using AutoMapper;
using FCG.Application.Users.Queries.ListarUsuarios;
using FGC.Core.Users.Repositories;
using MediatR;

namespace FCG.Application.Users.Queries.ListUsers;

public class ListUsersQueryHandler : IRequestHandler<ListUsersQuery, List<UserDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public ListUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<List<UserDto>> Handle(ListUsersQuery request, CancellationToken cancellationToken)
    {
        var usuarios = await _userRepository.ListAsync();
        return usuarios.Select(u =>
            new UserDto(
                u.Id,
                u.Nome,
                u.Email.Value,          
                u.Papel.ToString()
            )).ToList();
    }
}


