using AutoMapper;
using FCG.Application.Tests.Users.TestData;
using FGC.Application.Users.Commands.UpdateProfile;
using FGC.Core.Exceptions;
using FGC.Core.Users.Entities;
using FGC.Core.Users.Repositories;
using FGC.Core.Users.ValueObjects;
using FluentAssertions;
using Moq;

namespace FCG.Application.Tests.Users;

public class UpdateProfileCommandHandlerTests
{
    private readonly Mock<IUserRepository> _usuarioRepo;
    private readonly Mock<IMapper> _mapper;
    private readonly UpdateProfileCommandHandler _handler;
    private readonly UpdateProfileCommandTestData _fixture;
    private Usuario _capturedUser;

    public UpdateProfileCommandHandlerTests()
    {
        _usuarioRepo = new Mock<IUserRepository>();
        _mapper = new Mock<IMapper>();
        _handler = new UpdateProfileCommandHandler(
            _usuarioRepo.Object,
            _mapper.Object
        );
        _fixture = new UpdateProfileCommandTestData();

        _usuarioRepo
            .Setup(r => r.UpdateAsync(It.IsAny<Usuario>()))
            .Callback<Usuario>(u => _capturedUser = u)
            .Returns(Task.CompletedTask);

        _mapper
            .Setup(m => m.Map<UpdateProfileResponseDto>(It.IsAny<Usuario>()))
            .Returns<Usuario>(u => new UpdateProfileResponseDto(
                Id: u.Id,
                Nome: u.Nome,
                Email: u.Email.Value
            ));
    }

    [Fact]
    public async Task Handle_UsuarioNaoEncontrado_DeveLancarDomainException()
    {
        // Arrange
        _usuarioRepo
            .Setup(r => r.GetByIdAsync(_fixture.UsuarioId))
            .ReturnsAsync((Usuario?)null);

        // Act
        Func<Task> act = () => _handler.Handle(_fixture.Request, CancellationToken.None);

        // Assert
        await act.Should()
                 .ThrowAsync<DomainException>()
                 .WithMessage("Usuário não encontrado.");

        _usuarioRepo.Verify(r => r.UpdateAsync(It.IsAny<Usuario>()), Times.Never);
        _mapper.Verify(m => m.Map<UpdateProfileResponseDto>(It.IsAny<Usuario>()), Times.Never);
    }
   
}