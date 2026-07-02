using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolAPI.Models.People;
namespace SchoolAPI.Data;

public class TeacherSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<SchoolDbContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        if (await dbContext.Teachers.AnyAsync()) return;

        var faker = new Faker();

        var teacherSeeds = new[]
        {
            new { Name = "Sokha Chan",    Specialization = "Khmer language" },
            new { Name = "Dara Lim",      Specialization = "Mathematics" },
            new { Name = "Sreyneang Kim", Specialization = "English" },
            new { Name = "Vannak Heng",   Specialization = "Chemistry" },
            new { Name = "Piseth Ouk",    Specialization = "History" },
            new { Name = "Bopha Chea",    Specialization = "Geography" },
            new { Name = "Rithy Nhem",    Specialization = "Physical Education" },
            new { Name = "Sopheak Mean",  Specialization = "Civics and Moral education" },
            new { Name = "Malis Touch",   Specialization = "Biology" },
            new { Name = "Visal Keo",     Specialization = "Physical Education" },
        };

        var teachers = new List<Teacher>();

        foreach (var seed in teacherSeeds)
        {
            var slug = seed.Name.ToLowerInvariant().Replace(" ", "");
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = $"{slug}@example.com",
                Email = $"{slug}@example.com",
                EmailConfirmed = true,
            };

            var result = await userManager.CreateAsync(user, "Teacher@123");
            if (!result.Succeeded)
                throw new Exception(
                    $"Failed to create user for '{seed.Name}': {string.Join(", ", result.Errors.Select(e => e.Description))}");

            teachers.Add(new Teacher
            {
                Id = Guid.NewGuid().ToString(),
                Name = seed.Name,
                Specialization = seed.Specialization,
                Phone = faker.Phone.PhoneNumber("010-###-###"), // NEW: Faker for the non-meaningful field
                UserId = user.Id
            });
        }

        await dbContext.Teachers.AddRangeAsync(teachers);
        await dbContext.SaveChangesAsync();
        Console.WriteLine($"Seeded {teachers.Count} teachers.");
    }
}