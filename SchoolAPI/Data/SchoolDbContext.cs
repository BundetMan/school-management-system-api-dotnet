namespace SchoolAPI.Data;
using Microsoft.EntityFrameworkCore;
using SchoolAPI.Models.Curriculum_Bridges;
using SchoolAPI.Models.PaymentsWaitlists;
using SchoolAPI.Models.People;
using SchoolAPI.Models.Registrations;
using SchoolAPI.Models.Schedules;
using SchoolAPI.Models.School_Structure;

public class SchoolDbContext(DbContextOptions<SchoolDbContext> options) : DbContext(options)
{
    // School Structure
    public DbSet<SchoolLevel> SchoolLevels { get; set; }
    public DbSet<Level> Levels { get; set; }
    public DbSet<Class> Classes { get; set; }

    // People
    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<User> Users { get; set; }

    // Curriculum Bridges
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<ClassSubject> ClassSubjects { get; set; }
    public DbSet<TeacherSubjectClass> TeacherSubjectClasses { get; set; }

    // Registrations
    public DbSet<RegistrationStatus> RegistrationStatuses { get; set; }
    public DbSet<Registration> Registrations { get; set; }

    // Payments & Waitlists
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Waitlist> Waitlists { get; set; }

    // Schedules
    public DbSet<Schedule> Schedules { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}
