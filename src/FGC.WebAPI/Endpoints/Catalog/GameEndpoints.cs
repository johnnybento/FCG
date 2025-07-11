using FGC.Application.Catalog.Commands.CreateGame;
using FGC.Application.Catalog.Queries.ListGames;
using MediatR;


namespace FGC.WebAPI.Endpoints.Catalog;

public static class GameEndpoints
{
    public static WebApplication MapGameEndpoints(this WebApplication app)
    {
        var games = app.MapGroup("/api/jogos")
            .RequireAuthorization();

        // POST /api/jogos
        games.MapPost("",
                async (CreateGameCommand cmd, IMediator mediator) =>
                {
                    var dto = await mediator.Send(cmd);
                    return Results.Created($"/api/jogos/{dto.Id}", dto);
                })
            .RequireAuthorization(policy => policy.RequireRole("Administrador"))
            .Accepts<CreateGameCommand>("application/json")
            .Produces<CreateGameResponseDto>(201)
            .Produces(400);

        // GET /api/jogos
        games.MapGet("",
                async (IMediator mediator) =>
                {
                    var lista = await mediator.Send(new ListGamesQuery());
                    return Results.Ok(lista);
                })
            .WithName("ListGames")
            .Produces<List<GameDto>>(200);

        return app;
    }
}