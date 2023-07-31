using KundenBlazorApp.Shared;
using Microsoft.AspNetCore.Identity;

namespace KundenBlazorApp.Server.Data
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "zxc";
            string password = "zxc";

            foreach (Role role in Enum.GetValues(typeof(Role)))
            {
                string str = Enum.GetName(typeof(Role), role);
                if (!string.IsNullOrEmpty(str))
                {
                    if (await roleManager.FindByNameAsync(str) == null)
                    {
                        await roleManager.CreateAsync(
                            new IdentityRole
                            {
                                Name = str,
                                NormalizedName = str.ToUpper()
                            });
                    }
                }
            }

            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                ApplicationUser admin = new ApplicationUser 
                { 
                    Email = adminEmail, 
                    UserName = adminEmail 
                };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, Role.pichugina.ToString());
                    await userManager.AddToRoleAsync(admin, Role.admin.ToString());
                    await userManager.AddToRoleAsync(admin, Role.user.ToString());
                }
            }
        }
    }
}
