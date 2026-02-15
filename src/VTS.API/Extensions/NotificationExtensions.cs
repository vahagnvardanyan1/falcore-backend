using VTS.API.Services;
using VTS.BLL.Interfaces;

namespace VTS.API.Extensions;

public static class NotificationExtensions
{
    public static IServiceCollection AddNotificationServices(this IServiceCollection services)
    {
        services.AddCors();
        services.AddSignalR();
        services.AddScoped<INotificationBroadcaster, NotificationBroadcaster>();

        return services;
    }
}
