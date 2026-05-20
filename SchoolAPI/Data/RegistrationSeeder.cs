using Bogus;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolAPI.Models.People;
using SchoolAPI.Models.Registrations;
using System.Net.NetworkInformation;

namespace SchoolAPI.Data
{
    public class RegistrationSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<SchoolDbContext>();

            if (await dbContext.Registrations.AnyAsync()) return;

            var students = await dbContext.Students.ToListAsync();
            var classes = await dbContext.Classes.ToListAsync();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var admin = await userManager.FindByNameAsync("admin");

            if (!students.Any() || !classes.Any())
            {
                Console.WriteLine("Seed students and classes first.");
                return;
            }
            var faker = new Faker();

            for (int i = 0; i < 20; i++)
            {
                var student = faker.PickRandom(students);
                var classEntity = faker.PickRandom(classes);
                // Avoid duplicate registrations
                if (await dbContext.Registrations.AnyAsync(r => r.StudentId == student.Id && r.ClassId == classEntity.Id))
                    continue;

                var status = faker.PickRandom<RegistrationStatus>();
                var createdAt = faker.Date.Past(1);

                var registration = new Registration
                {
                    Id = Guid.NewGuid().ToString(),
                    StudentId = student.Id,
                    ClassId = classEntity.Id,
                    Status = status,
                    CreatedAt = createdAt,
                    ProcessedBy = admin?.Id,        
                    ProcessedAt = createdAt,
                    EnrolledBy = status == RegistrationStatus.Approved ? admin?.Id : null,
                    EnrolledAt = status == RegistrationStatus.Approved ? createdAt : default,
                    RejectedBy = status == RegistrationStatus.Rejected ? admin?.Id : null,
                    RejectedAt = status == RegistrationStatus.Rejected ? createdAt : null,
                };
                dbContext.Registrations.Add(registration);

                if(registration.Status == RegistrationStatus.Approved)
                {
                    dbContext.Enrollments.Add(new Models.Enrollment.Enrollment
                    {
                        Id = Guid.NewGuid().ToString(),
                        StudentId = student.Id,
                        RegistrationId = registration.Id,
                        ClassId = classEntity.Id,
                        EnrolledAt = createdAt,
                    });
                }
            }
            await dbContext.SaveChangesAsync();
        }
    }
}
