using Microsoft.AspNetCore.SignalR;

namespace VTS.API.Hubs;

public class NotificationHub : Hub
{
    public Task RegisterTenant(long tenantId)
    {
        var groupName = $"tenant-{tenantId}";
        return Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public Task UnregisterTenant(long tenantId)
    {
        var groupName = $"tenant-{tenantId}";
        return Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }
}