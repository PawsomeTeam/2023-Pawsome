using Microsoft.AspNetCore.Identity;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Server.Data;

public class IdentitySeedData
{
    public static async Task Initialize(UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        string[] roles =
        {
            "Admin",
            "AnimalManager",
            "ProductManager",
            "User"
        };

        foreach (var role in roles)
        {
            if (await roleManager.FindByNameAsync(role) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        string admin = "Admin";
        string adminEmail = "admin@admin.com";
        string adminPass = "Admin123!";
        if (userManager.FindByNameAsync(admin).Result == null)
        {
            User user = new User();
            user.FirstName = admin;
            user.LastName = admin;
            user.UserName = admin;
            user.Email = adminEmail;
            user.PhoneNumber = "7777777777";
            IdentityResult result = userManager.CreateAsync(user, adminPass).Result;

            if (result.Succeeded)
            {
                var result2 = userManager.AddToRoleAsync(user, "Admin").Result;
                // if (!result2.Succeeded)
                // FIXME: log the error
            }
        }
    }
    
}