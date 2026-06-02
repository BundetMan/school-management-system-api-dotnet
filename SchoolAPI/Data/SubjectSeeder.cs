using Microsoft.EntityFrameworkCore;
using SchoolAPI.Models.SubjectAndBridge;
namespace SchoolAPI.Data;

public class SubjectSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<SchoolDbContext>();
        if (await dbContext.Subjects.AnyAsync()) return;
        var subjects = new List<Subject>
        {
            new Subject { Id = Guid.NewGuid().ToString(), Name = "Mathematics", Code = "MATH" },
            new Subject { Id = Guid.NewGuid().ToString(), Name = "English", Code = "ENG" },
            new Subject { Id = Guid.NewGuid().ToString(), Name = "Science", Code = "SCI" },
            new Subject { Id = Guid.NewGuid().ToString(), Name = "History", Code = "HIS" },
            new Subject { Id = Guid.NewGuid().ToString(), Name = "Geography", Code = "GEO" },
            new Subject { Id = Guid.NewGuid().ToString(), Name = "Physical Education", Code = "PE" },
            new Subject { Id = Guid.NewGuid().ToString(), Name = "Art", Code = "ART" },
            new Subject { Id = Guid.NewGuid().ToString(), Name = "Music", Code = "MUS" },
            new Subject { Id = Guid.NewGuid().ToString(), Name = "Computer Science", Code = "CS" }
        };
        await dbContext.Subjects.AddRangeAsync(subjects);
        await dbContext.SaveChangesAsync();
        Console.WriteLine("Seeded subjects.");
    }
}