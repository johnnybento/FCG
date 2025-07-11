using FGC.Application.Sale.Commands.ComprarJogo;
using MediatR;

namespace FGC.WebAPI.Endpoints.Sales;

public static class PurchaseEndpoints
{
    public static WebApplication MapPurchaseEndpoints(this WebApplication app)
    {
        // Compra de jogo
        var buy = app.MapGroup("/api/sales/{usuarioId:guid}/comprar")
            .RequireAuthorization();

        buy.MapPost("{jogoId:guid}",
                async (Guid usuarioId, Guid jogoId, IMediator mediator) =>
                {
                    var cmd = new ComprarJogoCommand(usuarioId, jogoId);
                    var dto = await mediator.Send(cmd);
                    return Results.Ok(dto);
                })
            .WithName("PurchaseGame")
            .Produces<ComprarJogoResponseDto>(200)
            .Produces(400);

        return app;
    }
}
