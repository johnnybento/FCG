using FGC.Application.Common.Ports;
using FGC.Core.Catalog.Events;
using FGC.Core.Catalog.Repositories;
using FGC.Core.Sale.Events;
using FGC.Core.Sale.Repositories;
using FGC.Core.Users.Events;
using FGC.Core.Users.Repositories;
using FGC.Infrastructure.Data;
using FGC.Infrastructure.Repositories.Catalog;
using FGC.Infrastructure.Repositories.Sales;
using FGC.Infrastructure.Repositories.Users;
using FGC.Infrastructure.Services.Email;
using FGC.Infrastructure.Services.Hashing;
using FGC.Infrastructure.Services.Jwt;
using FGC.Application.Events;
using FGC.Core.Common.Events.Interfaces;
using FGC.Infrastructure.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FGC.Infrastructure.IoC;

public static class InfrastructureModuleInitializer
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        IConfiguration configurationAppsettings = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();            
        services.AddDbContext<ApplicationDBContext>(options =>
            options.UseSqlServer(configurationAppsettings.GetConnectionString("ConnectionString")));

        // Repositórios específicos
        services.AddScoped<IUserRepository, UsuarioRepository>();
        services.AddScoped<IJogoRepository, JogoRepository>();
        services.AddScoped<IPromocaoRepository, PromocaoRepository>();
        services.AddScoped<IJogoAdquiridoRepository, JogoAdquiridoRepository>();



        // Eventos de domínio
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddScoped<INotificationHandler<DomainEventNotification<UsuarioCadastradoEvent>>, SendWelcomeEmailHandler>();
        services.AddScoped<INotificationHandler<DomainEventNotification<PromocaoCriadaEvent>>, PromocaoCriadaHandler>();
        services.AddScoped<INotificationHandler<DomainEventNotification<JogoAdquiridoNaBibliotecaEvent>>, JogoAdquiridoNaBibliotecaHandler>();
        services.AddScoped<INotificationHandler<DomainEventNotification<JogoCadastradoEvent>>, JogoLancadoPlataformaHandler>();
        services.AddScoped<INotificationHandler<DomainEventNotification<PerfilAtualizadoEvent>>, PerfilAtualizadoHandler>();
        services.AddScoped<INotificationHandler<DomainEventNotification<SenhaAlteradaEvent>>, SenhaAlteradaHandler>();
        services.AddScoped<INotificationHandler<DomainEventNotification<UsuarioDesativadoEvent>>, UsuarioDesativadoHandler>();

        // Serviços de domínio
        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
        services.AddSingleton<IJwtService, JwtService>();
        services.AddTransient<IEmailSender, SmtpEmailSender>();

        return services;
    }
}