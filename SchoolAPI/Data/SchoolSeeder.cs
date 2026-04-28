using SchoolAPI.Models.School_Structure;
namespace SchoolAPI.Data;

public static class SchoolSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<SchoolDbContext>();

        // Ensure database is created
        await context.Database.EnsureCreatedAsync();

        // 1. Seed SchoolLevels
        if (!context.SchoolLevels.Any())
        {
            var primary = new SchoolLevel { Id = Guid.NewGuid().ToString(), Name = "Primary School"};
            var secondary = new SchoolLevel { Id = Guid.NewGuid().ToString(), Name = "Secondary School"};
            var high = new SchoolLevel { Id = Guid.NewGuid().ToString(), Name = "High School"};

            await context.SchoolLevels.AddRangeAsync(primary, secondary, high);
            await context.SaveChangesAsync();

            // 2. Seed Levels
            var grade1 = new Level { Id = Guid.NewGuid().ToString(), Name = "Grade 1", SchoolLevelId = primary.Id };
            var grade7 = new Level { Id = Guid.NewGuid().ToString(), Name = "Grade 7", SchoolLevelId = secondary.Id };
            var grade10 = new Level { Id = Guid.NewGuid().ToString(), Name = "Grade 10", SchoolLevelId = high.Id };

            await context.Levels.AddRangeAsync(grade1, grade7, grade10);
            await context.SaveChangesAsync();

            // 3. Seed Classes
            var class1A = new Class { Id = Guid.NewGuid().ToString(), Name = "1A", LevelId = grade1.Id };
            var class7B = new Class { Id = Guid.NewGuid().ToString(), Name = "7B", LevelId = grade7.Id };
            var class10C = new Class { Id = Guid.NewGuid().ToString(), Name = "10C", LevelId = grade10.Id };

            await context.Classes.AddRangeAsync(class1A, class7B, class10C);
            await context.SaveChangesAsync();
        }
    }
}
