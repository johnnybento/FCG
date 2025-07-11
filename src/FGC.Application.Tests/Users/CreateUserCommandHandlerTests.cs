using AutoMapper;
using FCG.Application.Tests.Users.TestData;
using FGC.Application.Common.Ports;
using FGC.Application.Users.Commands.CreateUser;
using FGC.Core.Exceptions;
using FGC.Core.Users.Entities;
using FGC.Core.Users.Repositories;
using FluentAssertions;
using Moq;

namespace FCG.Application.Tests.Users;

public class CreateUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _usuarioRepo;
    private readonly Mock<IPasswordHasher> _passwordHasher;
    private readonly Mock<IMapper> _mapper;
    private readonly CreateUserCommandHandler _handler;
    private readonly CreateUserCommandTestData _fixture;
    private Usuario _capturedUser;

    public CreateUserCommandHandlerTests()
    {
        _usuarioRepo = new Mock<IUserRepository>();
        _passwordHasher = new Mock<IPasswordHasher>();
        _mapper = new Mock<IMapper>();
        _handler = new CreateUserCommandHandler(
            _usuarioRepo.Object,
            _passwordHasher.Object,
            _mapper.Object
        );
        _fixture = new CreateUserCommandTestData();

        _usuarioRepo
            .Setup(r => r.AddAsync(It.IsAny<Usuario>()))
            .Callback<Usuario>(u => _capturedUser = u)
            .Returns(Task.CompletedTask);

        _mapper
            .Setup(m => m.Map<CreateUserResponseDto>(It.IsAny<Usuario>()))
            .Returns<Usuario>(u => new CreateUserResponseDto(
                Id: u.Id,
                Nome: u.Nome,
                Email: u.Email.Value
            ));
    }

    [Fact]
    public async Task Handle_EmailJáExiste_DeveLancarDomainException()
    {
        // Arrange
        _usuarioRepo
            .Setup(r => r.EmailExistsAsync(_fixture.Request.Email))
            .ReturnsAsync(true);

        // Act
        Func<Task> act = () => _handler.Handle(_fixture.Request, CancellationToken.None);

        // Assert
        await act.Should()
                 .ThrowAsync<DomainException>()
                 .WithMessage("Email já cadastrado.");

        _passwordHasher.Verify(h => h.Hash(It.IsAny<string>()), Times.Never);
        _usuarioRepo.Verify(r => r.AddAsync(It.IsAny<Usuario>()), Times.Never);
        _mapper.Verify(m => m.Map<CreateUserResponseDto>(It.IsAny<Usuario>()), Times.Never);
    }

    
}