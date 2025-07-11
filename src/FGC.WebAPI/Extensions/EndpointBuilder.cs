using MediatR;

namespace FGC.WebAPI.Extensions;
public static class EndpointBuilder
{
    /// <summary>
    /// Mapeia um POST de comando MediatR que retorna TResult.
    /// </summary>
    public static RouteHandlerBuilder MapPostCommand<TCommand, TResult>(
        this RouteGroupBuilder group,
        string pattern,
        bool allowAnonymous = false)
        where TCommand : IRequest<TResult>
    {
        var builder = group.MapPost(pattern,
                async (TCommand cmd, IMediator mediator) =>
                {
                    var result = await mediator.Send(cmd);
                    return Results.Ok(result);
                })
            .Accepts<TCommand>("application/json")
            .Produces<TResult>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        if (allowAnonymous)
            builder.AllowAnonymous();
        else
            builder.RequireAuthorization();

        return builder;
    }

    /// <summary>
    /// Mapeia um POST de comando MediatR que retorna void (204).
    /// </summary>
    public static RouteHandlerBuilder MapPostCommand<TCommand>(
        this RouteGroupBuilder group,
        string pattern,
        bool allowAnonymous = false)
        where TCommand : IRequest
    {
        var builder = group.MapPost(pattern,
                async (TCommand cmd, IMediator mediator) =>
                {
                    await mediator.Send(cmd);
                    return Results.NoContent();
                })
            .Accepts<TCommand>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest);

        if (allowAnonymous)
            builder.AllowAnonymous();
        else
            builder.RequireAuthorization();

        return builder;
    }

}