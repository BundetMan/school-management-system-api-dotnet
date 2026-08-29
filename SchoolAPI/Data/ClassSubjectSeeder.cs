using Microsoft.EntityFrameworkCore;
using SchoolAPI.Models.SubjectAndBridge;
namespace SchoolAPI.Data;

public class ClassSubjectSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<SchoolDbContext>();

        if (await dbContext.ClassSubjects.AnyAsync()) return;

        var classes = await dbContext.Classes.ToListAsync();
        var subjects = await dbContext.Subjects.ToListAsync();

        if (classes.Count == 0 || subjects.Count == 0)
        {
            Console.WriteLine("Skipping ClassSubject seed: Classes or Subjects not yet seeded.");
            return;
        }

        const int subjectsPerClass = 10; // adjust between 2–10
        var classSubjects = new List<ClassSubject>();
        foreach (var (cls, ci) in classes.Select((c, i) => (c, i)))
        {
            // Pick a different slice of subjects for each class, wrapping around
            var assignedSubjects = Enumerable.Range(0, subjectsPerClass)
                .Select(i => subjects[(ci + i) % subjects.Count])
                .DistinctBy(s => s.Id) // prevent duplicates within a class
                .ToList();

            foreach (var subject in assignedSubjects)
            {
                classSubjects.Add(new ClassSubject
                {
                    Id = Guid.NewGuid().ToString(),
                    ClassId = cls.Id,
                    SubjectId = subject.Id
                });
            }
        }

        await dbContext.ClassSubjects.AddRangeAsync(classSubjects);
        await dbContext.SaveChangesAsync();

        Console.WriteLine($"Seeded {classSubjects.Count} class-subject relationships.");
    }
}
