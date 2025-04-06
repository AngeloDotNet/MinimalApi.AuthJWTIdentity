using Microsoft.AspNetCore.Identity;

namespace Shared;

public class ApplicationRole : IdentityRole<Guid>
{
    public ApplicationRole()
    { }

    public ApplicationRole(string roleName) : base(roleName)
    { }

    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
}