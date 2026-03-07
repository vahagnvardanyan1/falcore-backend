using Microsoft.AspNetCore.Identity;

namespace VTS.DAL.Entities;

public class ApplicationRole : IdentityRole<long>
{
    public string Description { get; set; }
    public ICollection<ApplicationUserRole> UserRoles { get; set; } = [];
}
