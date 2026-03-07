using Microsoft.AspNetCore.Identity;
using VTS.Common.Enums;
using VTS.DAL.Entities;

namespace VTS.DAL.Seeds;

public static class RoleSeeder
{
    public static async Task SeedRolesAsync(RoleManager<ApplicationRole> roleManager)
    {
        var roles = new[]
        {
            new ApplicationRole
            {
                Name = RolesEnum.SuperAdmin.ToString(),
                NormalizedName = RolesEnum.SuperAdmin.ToString().ToUpperInvariant(),
                Description = "Has access to all tenants and all vehicles"
            },
            new ApplicationRole
            {
                Name = RolesEnum.TenantAdmin.ToString(),
                NormalizedName = RolesEnum.TenantAdmin.ToString().ToUpperInvariant(),
                Description = "Has administrative access to assigned tenant(s)"
            },
            new ApplicationRole
            {
                Name = RolesEnum.Viewer.ToString(),
                NormalizedName = RolesEnum.Viewer.ToString().ToUpperInvariant(),
                Description = "Can view assigned tenants and vehicles"
            }
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role.Name))
            {
                await roleManager.CreateAsync(role);
            }
        }
    }
}
