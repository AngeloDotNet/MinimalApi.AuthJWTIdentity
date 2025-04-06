using Microsoft.AspNetCore.Identity;

namespace Shared;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpirationDate { get; set; }
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();
}