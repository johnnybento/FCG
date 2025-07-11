using FGC.Application.Auth.Commands.Login;
using MediatR;

namespace FGC.WebApi.Endpoints.Auth;

public static class AuthEndpoints
{
    public static WebApplication MapAuthEndpoints(this WebApplication app)
    {
        var auth = app.MapGroup("/api/auth");

        // POST /api/auth/login
        auth.MapPost("/login",
                async (LoginCommand cmd, IMediator mediator) =>
                {
                    var result = await mediator.Send(cmd);
                    return Results.Ok(result);
                })
            .AllowAnonymous()
            .WithName("Login")
            .Accepts<LoginCommand>("application/json")
            .Produces<LoginResultDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        return app;
    }
}