using FGC.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace FGC.WebAPI.Middleware;

/// <summary>
/// Middleware para capturar exceções de domínio, validação e outras, retornando ProblemDetails.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DomainException dex)
        {
            _logger.LogWarning(dex, "Erro de dóminio");
            await HandleProblemAsync(context, HttpStatusCode.BadRequest, dex.Message);
        }
        catch (FluentValidation.ValidationException vex)
        {
            _logger.LogWarning(vex, "Validation failure");
            var errors = vex.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );
            await HandleProblemAsync(context, (HttpStatusCode)422, "Validation errors", errors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await HandleProblemAsync(context, HttpStatusCode.InternalServerError, "Ocorreu um erro interno no servidor.");
        }
    }
    private static async Task HandleProblemAsync(HttpContext context, HttpStatusCode statusCode, string title, object? details = null)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)statusCode;

        var problem = new ProblemDetails
        {
            Status = (int)statusCode,
            Title = title,
            Detail = details is string ? (string)details : null
        };

        if (details is not string && details is object)
        {            
            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            var dict = new
            {
                problem.Status,
                problem.Title,
                errors = details
            };
            await context.Response.WriteAsJsonAsync(dict, options);
        }
        else
        {
            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}