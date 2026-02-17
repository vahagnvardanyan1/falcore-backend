using Microsoft.OpenApi.Models;

namespace VTS.API.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "VTS API", Version = "v1" });
        });

        return services;
    }

    public static WebApplication MapSwaggerDocumentation(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "VTS API v1");
            c.RoutePrefix = string.Empty;
            c.EnableTryItOutByDefault();
            c.DisplayOperationId();
            c.DisplayRequestDuration();
        });

        return app;
    }
}
