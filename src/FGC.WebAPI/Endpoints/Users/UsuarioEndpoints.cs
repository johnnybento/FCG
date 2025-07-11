using FCG.Application.Users.Queries.ListarUsuarios;
using FCG.Application.Users.Queries.ListUsers;
using FGC.Application.Users.Commands.ChangePassword;
using FGC.Application.Users.Commands.CreateUser;
using FGC.Application.Users.Commands.DeactiveUser;
using FGC.Application.Users.Commands.UpdateProfile;
using FGC.Application.Users.Queries.GetUserById;
using FGC.Application.Users.Queries.ListLibrary;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace FGC.WebApi.Endpoints.Users;

public static class UsuarioEndpoints
{
    public static WebApplication MapUsuarioEndpoints(this WebApplication app)
    {
        var users = app.MapGroup("/api/usuarios");

        // POST /api/usuarios (público)
        users.MapPost("",
                async (CreateUserCommand cmd, IMediator m) =>
                {
                    var dto = await m.Send(cmd);
                    return Results.Created($"/api/usuarios/{dto.Id}", dto);
                })
            .AllowAnonymous()
            .Accepts<CreateUserCommand>("application/json")
            .Produces<CreateUserResponseDto>(201)
            .Produces(400);

        // GET /api/usuarios/{id}
        users.MapGet("{id:guid}",
                async (Guid id, IMediator m) =>
                {
                    var dto = await m.Send(new GetUserByIdQuery(id));
                    return dto is not null
                        ? Results.Ok(dto)
                        : Results.NotFound();
                })
            .RequireAuthorization()
            .WithName("GetUserById")
            .Produces<UserProfileDto>(200)
            .Produces(404);

        // PUT /api/usuarios/{id}
        users.MapPut("{id:guid}",
                async (Guid id, UpdateProfileCommand cmd, IMediator m) =>
                {
                    if (id != cmd.UsuarioId)
                        return Results.Forbid();

                    var dto = await m.Send(cmd);
                    return Results.Ok(dto);
                })
            .RequireAuthorization()
            .Accepts<UpdateProfileCommand>("application/json")
            .Produces<UpdateProfileResponseDto>(200)
            .Produces(403)
            .Produces(400);

        // PUT /api/usuarios/{id}/senha
        users.MapPut("{id:guid}/senha",
                async (Guid id, ChangePasswordCommand cmd, IMediator m) =>
                {
                    if (id != cmd.UsuarioId)
                        return Results.Forbid();

                    await m.Send(cmd);
                    return Results.NoContent();
                })
            .RequireAuthorization()
            .Accepts<ChangePasswordCommand>("application/json")
            .Produces(204)
            .Produces(403)
            .Produces(400);

        // DELETE /api/usuarios/{id}
        users.MapDelete("{id:guid}",
                [Authorize(Roles = "Administrador")] async (Guid id, IMediator m) =>
                {
                    await m.Send(new DeactivateUserCommand(id));
                    return Results.NoContent();
                })
            .Produces(204)
            .Produces(404)
            .Produces(403);

        // GET /api/usuarios/{id}/biblioteca
        users.MapGet("{id:guid}/biblioteca",
                async (Guid id, IMediator m) =>
                {
                    var lista = await m.Send(new ListLibraryQuery(id));
                    return Results.Ok(lista);
                })
            .RequireAuthorization()
            .WithName("ListUserLibrary")
            .Produces<List<LibraryItemDto>>(200);


        // GET /api/usuarios
        users.MapGet("",
              [Authorize(Roles = "Administrador")] async (IMediator mediator) =>
                {
                    var lista = await mediator.Send(new ListUsersQuery());
                    return Results.Ok(lista);
                })
            .RequireAuthorization()
            .WithName("ListUsers")
            .Produces<List<UserDto>>(200);

        return app;


   




    }
}
