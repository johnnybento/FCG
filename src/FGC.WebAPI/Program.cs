
using FGC.WebAPI.Extensions;
using FGC.Infrastructure.IoC;            
using FGC.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

// 1) Serviços genéricos
builder.Services.AddEndpointsApiExplorer();

// 2) Swagger / OpenAPI
builder.Services.AddSwaggerDocumentation();

// 3) Database (EF Core + SQL Server)
builder.Services.AddDatabase(builder.Configuration);

// 4) Infraestrutura (repositórios, hashing, JWT, e-mail)
builder.Services.AddInfrastructure(builder.Configuration);

// 5) Application (MediatR, FluentValidation, AutoMapper)
builder.Services.AddApplication();

// 6) JWT Authentication & Authorization
builder.Services.AddJwtAuth(builder.Configuration);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administrador", policy =>
    {
        policy.RequireAuthenticatedUser()
              .RequireRole("Administrador");
    });
});


var app = builder.Build();

// 7) Middleware de tratamento de erros
app.UseExceptionHandling();

// 8) Swagger em dev only
app.UseSwaggerIfDev();

// 9) Autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();

// 10) Mapeamento de todos os endpoints (Auth, Users, Catalog, Sales)
app.MapAllEndpoints();

app.Run();



