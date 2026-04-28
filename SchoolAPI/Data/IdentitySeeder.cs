
using Microsoft.AspNetCore.Identity;
using SchoolAPI.Models.People;
namespace SchoolAPI.Data;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var roleMgr = serviceProvider.GetRequiredService<RoleManager<Role>>();
        var userMgr = serviceProvider.GetRequiredService<UserManager<User>>();

        // 1. Define roles
        var roles = new[]
        {
        new Role { Id = Guid.NewGuid().ToString(), Name = "Admin", NormalizedName = "ADMIN" },
        new Role { Id = Guid.NewGuid().ToString(), Name = "User", NormalizedName = "USER" },
        new Role { Id = Guid.NewGuid().ToString(), Name = "Teacher", NormalizedName = "TEACHER" },
        new Role { Id = Guid.NewGuid().ToString(), Name = "Student", NormalizedName = "STUDENT" }
    };

        // 2. Create roles if not exist
        foreach (var role in roles)
        {
            if (!await roleMgr.RoleExistsAsync(role.Name!))
            {
                await roleMgr.CreateAsync(role);
            }
        }

        // 3. Admin user
        const string adminId = "ADMINC8F-D914-483D-BF41-7DA09ABAA4DC";
        const string adminUser = "admin";
        const string adminEmail = "admin@example.com";
        const string adminPass = "Admin123!"; // Change in production

        var admin = await userMgr.FindByNameAsync(adminUser);

        if (admin == null)
        {
            admin = new User
            {
                Id = adminId,
                UserName = adminUser,
                NormalizedUserName = adminUser.ToUpper(),
                Email = adminEmail,
                NormalizedEmail = adminEmail.ToUpper(),
                EmailConfirmed = true,
                Status = "Active"
            };

            var result = await userMgr.CreateAsync(admin, adminPass);

            if (result.Succeeded)
            {
                await userMgr.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}
