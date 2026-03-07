using Microsoft.AspNetCore.Identity;

namespace VTS.DAL.Entities;

public class ApplicationUserRole : IdentityUserRole<long>
{
    public virtual ApplicationUser User { get; set; }
    public virtual ApplicationRole Role { get; set; }
}
