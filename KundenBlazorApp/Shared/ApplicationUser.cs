using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace KundenBlazorApp.Shared
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(32)]
        public string? AvatarName { get; set; }

    }

    public enum Role
    {
        user = 0,
        pichugina = 0x01,
        admin = 0x02,
        creator = 0x04,
    }
}
