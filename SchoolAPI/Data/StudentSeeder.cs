using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolAPI.Models;
using SchoolAPI.Models.People;
using static Bogus.DataSets.Name;
namespace SchoolAPI.Data;

public class StudentSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<SchoolDbContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

        if (await dbContext.Students.AnyAsync()) return;

        var levels = await dbContext.Levels.ToListAsync();
        var classes = await dbContext.Classes.ToListAsync();

        if (!levels.Any() || !classes.Any())
        {
            Console.WriteLine("Seed levels and classes first.");
            return;
        }

        var faker = new Faker();

        for (int i = 0; i < 50; i++)
        {
            var fullName = faker.Name.FullName();
            var email = faker.Internet.Email();

            // Create user first
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = email,
                Email = email,
                EmailConfirmed = false,
                Status = "Active"
            };

            var result = await userManager.CreateAsync(user, "Password123!");
            if (!result.Succeeded) continue;

            // Then create student with that userId
            var student = new Student
            {
                Id = Guid.NewGuid().ToString(),
                Code = $"ST-{faker.Random.AlphaNumeric(8).ToUpper()}",
                FullName = fullName,
                LatinName = faker.Name.FullName(),
                Gender = faker.PickRandom<GenderType>(),
                Status = StudentStatus.Active,
                DateOfBirth = faker.Date.Past(20, DateTime.Now.AddYears(-5)),
                PlaceOfBirth = faker.Address.City(),
                BackgroundStudy = faker.Lorem.Sentence(),
                FatherName = faker.Name.FullName(),
                MotherName = faker.Name.FullName(),
                Contact = faker.Phone.PhoneNumber(),
                Address = faker.Address.FullAddress(),
                UserId = user.Id
            };

            await dbContext.Students.AddAsync(student);
        }

        await dbContext.SaveChangesAsync();
        Console.WriteLine("Seeded 50 students with users.");
    }
}