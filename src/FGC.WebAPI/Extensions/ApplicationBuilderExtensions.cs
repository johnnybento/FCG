using FGC.WebApi.Endpoints.Auth;
using FGC.WebApi.Endpoints.Users;
using FGC.WebAPI.Endpoints.Catalog;
using FGC.WebAPI.Endpoints.Sales;
using FGC.WebAPI.Middleware;

namespace FGC.WebAPI.Extensions;

public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Aplica Swagger e SwaggerUI apenas em ambiente de desenvolvimento.
    /// </summary>
    public static WebApplication UseSwaggerIfDev(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        return app;
    }

    /// <summary>
    /// Registra middleware global de tratamento de exceções.
    /// </summary>
    public static WebApplication UseExceptionHandling(this WebApplication app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        return app;
    }

    /// <summary>
    /// Mapeia todos os grupos de endpoints da API.
    /// </summary>
    public static WebApplication MapAllEndpoints(this WebApplication app)
    {
        app.MapAuthEndpoints();
        app.MapUsuarioEndpoints();
        app.MapGameEndpoints();
        app.MapPromotionEndpoints();
        app.MapPurchaseEndpoints();
        return app;

    }

    /// <summary>
    /// Configura o CORS para permitir requisições de qualquer origem.
    /// </summary>
    public static WebApplication UseCorsAllowAll(this WebApplication app)
    {
        app.UseCors(builder => builder.AllowAnyOrigin()
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
        return app;
    }   
}   
