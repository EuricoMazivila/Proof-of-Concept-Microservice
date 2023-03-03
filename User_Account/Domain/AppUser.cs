using Microsoft.AspNetCore.Identity;

namespace Domain;

public class AppUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public bool Archived { get; set; }
}