using FGC.Application.Catalog.Commands.CreatePromotion;
using FGC.Application.Catalog.Queries.ListPromotions;
using MediatR;


namespace FGC.WebAPI.Endpoints.Catalog;

public static class PromotionEndpoints
{
    public static WebApplication MapPromotionEndpoints(this WebApplication app)
    {
        // Criação de promoção (Admin)
        var prom = app.MapGroup("/api/promocoes")
            .RequireAuthorization("Administrador");

        prom.MapPost("",
                async (CreatePromotionCommand cmd, IMediator mediator) =>
                {
                    var dto = await mediator.Send(cmd);
                    return Results.Created($"/api/promocoes/{dto.Id}", dto);
                })
            .Accepts<CreatePromotionCommand>("application/json")
            .Produces<CreatePromotionResponseDto>(201)
            .Produces(400);

        // Listagem de promoções ativas por jogo
        var jogoProm = app.MapGroup("/api/jogos/{jogoId:guid}/promocoes")
            .RequireAuthorization();

        jogoProm.MapGet("",
                async (Guid jogoId, IMediator mediator) =>
                {
                    var lista = await mediator.Send(new ListPromotionsQuery(jogoId));
                    return Results.Ok(lista);
                })
            .WithName("ListPromotions")
            .Produces<List<PromotionDto>>(200)
            .Produces(404);

        return app;
    }
}
