using Microsoft.AspNetCore.Identity;
using VTS.Common.Enums;
using VTS.DAL.Entities;

namespace VTS.DAL.Seeds;

public static class UserSeeder
{
    public static async Task SeedSampleUsersAsync(
        UserManager<ApplicationUser> userManager)
    {
        // Seed SuperAdmin user
        var superAdminEmail = "admin@vts.local";
        var superAdminUser = await userManager.FindByEmailAsync(superAdminEmail);

        if (superAdminUser == null)
        {
            var newSuperAdmin = new ApplicationUser
            {
                UserName = superAdminEmail,
                Email = superAdminEmail,
                EmailConfirmed = true,
                FirstName = "System",
                LastName = "Administrator",
                IsActive = true,
                CreatedDateUtc = DateTime.UtcNow,
                ModifiedDateUtc = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(newSuperAdmin, "SuperAdmin@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newSuperAdmin, RolesEnum.SuperAdmin.ToString());
            }
        }

        // Seed TenantAdmin user
        var tenantAdminEmail = "tenant-admin@vts.local";
        var tenantAdminUser = await userManager.FindByEmailAsync(tenantAdminEmail);

        if (tenantAdminUser == null)
        {
            var newTenantAdmin = new ApplicationUser
            {
                UserName = tenantAdminEmail,
                Email = tenantAdminEmail,
                EmailConfirmed = true,
                FirstName = "Tenant",
                LastName = "Admin",
                IsActive = true,
                CreatedDateUtc = DateTime.UtcNow,
                ModifiedDateUtc = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(newTenantAdmin, "TenantAdmin@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newTenantAdmin, RolesEnum.TenantAdmin.ToString());
            }
        }

        // Seed Viewer user
        var viewerEmail = "viewer@vts.local";
        var viewerUser = await userManager.FindByEmailAsync(viewerEmail);

        if (viewerUser == null)
        {
            var newViewer = new ApplicationUser
            {
                UserName = viewerEmail,
                Email = viewerEmail,
                EmailConfirmed = true,
                FirstName = "Sample",
                LastName = "Viewer",
                IsActive = true,
                CreatedDateUtc = DateTime.UtcNow,
                ModifiedDateUtc = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(newViewer, "Viewer@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newViewer, RolesEnum.Viewer.ToString());
            }
        }
    }
}
