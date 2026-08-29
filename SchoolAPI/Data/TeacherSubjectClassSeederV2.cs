using Microsoft.EntityFrameworkCore;
using SchoolAPI.Models.SubjectAndBridge;
namespace SchoolAPI.Data;

public class TeacherSubjectClassSeederV2
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<SchoolDbContext>();

        if (await dbContext.TeacherSubjectClasses.AnyAsync())
            return;

        var teachers = await dbContext.Teachers.ToListAsync();
        var classSubjects = await dbContext.ClassSubjects
            .Include(cs => cs.Subject)
            .ToListAsync();

        if (teachers.Count == 0 || classSubjects.Count == 0)
        {
            Console.WriteLine("Skipping TeacherSubjectClass seed.");
            return;
        }

        var teacherSubjectClasses = new List<TeacherSubjectClass>();

        foreach (var classSubject in classSubjects)
        {
            var teacher = teachers.FirstOrDefault(t =>
                string.Equals(
                    t.Specialization,
                    classSubject.Subject.Name,
                    StringComparison.OrdinalIgnoreCase));

            if (teacher == null)
            {
                Console.WriteLine(
                    $"No teacher found for subject '{classSubject.Subject.Name}'.");
                continue;
            }

            teacherSubjectClasses.Add(new TeacherSubjectClass
            {
                Id = Guid.NewGuid().ToString(),
                TeacherId = teacher.Id,
                ClassSubjectId = classSubject.Id
            });
        }

        await dbContext.TeacherSubjectClasses.AddRangeAsync(teacherSubjectClasses);
        await dbContext.SaveChangesAsync();

        Console.WriteLine($"Seeded {teacherSubjectClasses.Count} TeacherSubjectClass records.");
    }
}