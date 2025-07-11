using FGC.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FGC.WebAPI.Extensions;
/// <summary>
/// Configura e obtém o acesso ao banco de dados.
/// </summary>
public static class DatabaseConfiguration
{   
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var appsettingsConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        services.AddDbContext<ApplicationDBContext>(options =>
        {
            options.UseSqlServer(appsettingsConfig.GetConnectionString("ConnectionString"));

        }, ServiceLifetime.Scoped);
        return services;
    }
}
