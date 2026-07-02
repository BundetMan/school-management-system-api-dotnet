using Microsoft.EntityFrameworkCore;
using SchoolAPI.Models.SubjectAndBridge;
namespace SchoolAPI.Data;

public class TeacherSubjectClassSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<SchoolDbContext>();

        if (await dbContext.TeacherSubjectClasses.AnyAsync()) return;

        var teachers = await dbContext.Teachers.ToListAsync();
        var classSubjects = await dbContext.ClassSubjects.ToListAsync();

        if (teachers.Count == 0 || classSubjects.Count == 0)
        {
            Console.WriteLine("Skipping TeacherSubjectClass seed: Teachers or ClassSubjects not yet seeded.");
            return;
        }

        // Assign each teacher to all class-subject combinations
        var teacherSubjectClasses = classSubjects
            .Select((cs, index) => new TeacherSubjectClass
            {
                Id = Guid.NewGuid().ToString(),
                TeacherId = teachers[index % teachers.Count].Id,
                ClassSubjectId = cs.Id
            })
            .ToList();

        await dbContext.TeacherSubjectClasses.AddRangeAsync(teacherSubjectClasses);
        await dbContext.SaveChangesAsync();

        Console.WriteLine($"Seeded {teacherSubjectClasses.Count} teacher-subject-class relationships.");
    }
}