using Microsoft.OpenApi.Models;

namespace FGC.WebAPI.Extensions;

public static class SwaggerConfiguration
{
    public static void AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "FGC API",
                Version = "v1",
               
            });
        });
    }

}
