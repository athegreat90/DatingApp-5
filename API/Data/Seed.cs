using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        private const string Password = "Pa$$w0rd";

        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync())
            {
                return;
            }

            var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

            if (users == null)
            {
                return;
            }

            var roles = new List<AppRole>()
            {
                new(){Name = "Member"},
                new(){Name = "Admin"},
                new(){Name = "Moderator"},
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }
            foreach (var user in users)
            {
                await userManager.CreateAsync(user, Password);
                await userManager.AddToRoleAsync(user, "Member");
            }

            var admin = new AppUser()
            {
                UserName = "admin"
            };
            await userManager.CreateAsync(admin, Password);
            await userManager.AddToRolesAsync(admin, new []{"Admin", "Moderator"});
        }
    }
}