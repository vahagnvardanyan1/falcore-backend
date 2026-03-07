using Microsoft.AspNetCore.Identity;

namespace VTS.DAL.Entities;

public class ApplicationUser : IdentityUser<long>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsActive { get; set; } = true;
    public string RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }
    public string PasswordResetToken { get; set; }
    public DateTime? PasswordResetTokenExpiry { get; set; }
    public DateTime CreatedDateUtc { get; set; }
    public DateTime ModifiedDateUtc { get; set; }

    public ICollection<ApplicationUserRole> UserRoles { get; set; } = [];
    public ICollection<UserTenantAccess> TenantAccesses { get; set; } = [];
    public ICollection<UserVehicleAccess> VehicleAccesses { get; set; } = [];
}
