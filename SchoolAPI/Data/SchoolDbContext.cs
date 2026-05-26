namespace SchoolAPI.Data;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolAPI.Models.Curriculum_Bridges;
using SchoolAPI.Models.Enrollment;
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
    //public DbSet<User> Users { get; set; }

    // Curriculum Bridges
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<ClassSubject> ClassSubjects { get; set; }
    public DbSet<TeacherSubjectClass> TeacherSubjectClasses { get; set; }

    // Registrations
    public DbSet<Registration> Registrations { get; set; }

    // Payments & Waitlists
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Waitlist> Waitlists { get; set; }

    // Schedules
    public DbSet<Schedule> Schedules { get; set; }

    //Enrollments
    public DbSet<Enrollment> Enrollments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region School Structure
        modelBuilder.Entity<SchoolLevel>(buildAction =>
        {
            buildAction.HasKey(s => s.Id);
            buildAction.Property(s => s.Id).HasColumnType("varchar(50)").IsRequired();
            buildAction.Property(s => s.Name).HasColumnType("nvarchar(50)").IsRequired();

            buildAction.HasIndex(s => s.Name).IsUnique().HasFilter("[Name] <> ''");

            buildAction.ToTable("SchoolLevels", t =>
            {
                t.HasCheckConstraint("CHK_SCHOOLLEVEL_NAME_NOT_EMPTY", "LEN(LTRIM(RTRIM([Name]))) > 0");
            });

        });

        modelBuilder.Entity<Level>(buildAction =>
        {
            buildAction.HasKey(l => l.Id);
            buildAction.Property(l => l.Id).HasColumnType("varchar(50)").IsRequired();
            buildAction.Property(l => l.Name).HasColumnType("nvarchar(50)").IsRequired();
            buildAction.Property(l => l.SchoolLevelId).HasColumnType("varchar(50)").IsRequired();

            buildAction.HasIndex(l => l.Name).IsUnique().HasFilter("[Name] <> ''");
            
            //Level -> SchoolLevel (Many-to-One)
            buildAction.HasOne(l => l.SchoolLevel)
                        .WithMany(c => c.Levels)
                        .HasForeignKey(l => l.SchoolLevelId)
                        .OnDelete(DeleteBehavior.Restrict);

            buildAction.ToTable("Levels", t =>
            {
                t.HasCheckConstraint("CHK_LEVEL_NAME_NOT_EMPTY", "LEN(LTRIM(RTRIM([Name]))) > 0");
            });
        });

        modelBuilder.Entity<Class>(buildAction =>
        {
            buildAction.HasKey(c => c.Id);
            buildAction.Property(c => c.Id).HasColumnType("varchar(50)").IsRequired();
            buildAction.Property(c => c.Name).HasColumnType("nvarchar(50)").IsRequired();
            buildAction.Property(c => c.Capacity).IsRequired().HasDefaultValue(50);
            buildAction.Property(c => c.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
            buildAction.Property(c => c.LevelId).HasColumnType("varchar(50)").IsRequired();
            buildAction.HasIndex(c => c.Name).IsUnique().HasFilter("[Name] <> ''");

            // Class → Level (Many-to-One)
            buildAction.HasOne(c => c.Level)
                        .WithMany(l => l.Classes)
                        .HasForeignKey(c => c.LevelId)
                        .OnDelete(DeleteBehavior.Restrict);

            // Class → ClassSubject (One-to-Many)
            buildAction.HasMany(c => c.ClassSubjects)
                       .WithOne(cs => cs.Class)
                       .HasForeignKey(cs => cs.ClassId)
                       .OnDelete(DeleteBehavior.Cascade);

            // Class → Schedule (One-to-Many)
            buildAction.HasMany(c => c.Schedules)
                       .WithOne(s => s.Class)
                       .HasForeignKey(s => s.ClassId)
                       .OnDelete(DeleteBehavior.Cascade);

            buildAction.ToTable("Classes", t =>
            {
                t.HasCheckConstraint("CHK_CLASS_NAME_NOT_EMPTY", "LEN(LTRIM(RTRIM([Name]))) > 0");
                t.HasCheckConstraint("CHK_CAPACITY_NOT_EMPTY", "[Capacity] > 0");
            });
        });

        #endregion
        #region People 
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Role>().ToTable("Roles");//default name is AspNetRoles
        modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles")
            .HasKey(x => new { x.UserId , x.RoleId});
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins").HasNoKey();
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens").HasNoKey();

        modelBuilder.Entity<Teacher>(buildAction =>
        {
            buildAction.HasKey(t => t.Id);
            buildAction.Property(t => t.Id).HasColumnType("varchar(50)").IsRequired();
            buildAction.Property(t => t.Name).HasColumnType("nvarchar(100)").IsRequired();
            buildAction.Property(t => t.Specialization).HasColumnType("nvarchar(50)");
            buildAction.Property(t => t.Phone).HasColumnType("varchar(20)");
            buildAction.Property(t => t.Gender).HasColumnType("varchar(10)");
            buildAction.Property(t => t.IsActive).HasDefaultValue(true);

            buildAction.Property(t => t.UserId).IsRequired();

            buildAction.HasIndex(t => t.Name).HasFilter("[Name] <> ''");

            buildAction.HasMany(t => t.TeacherSubjectClasses)
                       .WithOne(tsc => tsc.Teacher)
                       .HasForeignKey(tsc => tsc.TeacherId)
                       .OnDelete(DeleteBehavior.Restrict);

            buildAction.HasMany(t => t.Schedules)
                       .WithOne(s => s.Teacher)
                       .HasForeignKey(s => s.TeacherId)
                       .OnDelete(DeleteBehavior.Restrict);

            buildAction.ToTable("Teachers" , t =>
            {
                t.HasCheckConstraint(
                    "CHK_TEACHER_NAME_NOT_EMPTY",
                    "LEN(LTRIM(RTRIM([Name]))) > 0"
                );
            });
        });

        modelBuilder.Entity<Student>(buildAction =>
        {
            buildAction.HasKey(s => s.Id);
            buildAction.Property(s => s.Id).HasColumnType("varchar(50)").IsRequired();
            buildAction.Property(s => s.Code).HasColumnType("varchar(50)").IsRequired();
            buildAction.Property(s => s.FullName).HasColumnType("nvarchar(100)").IsRequired();
            buildAction.Property(s => s.LatinName).HasColumnType("nvarchar(100)").IsRequired();
            buildAction.Property(s => s.DateOfBirth).HasColumnType("date").IsRequired();
            buildAction.Property(s => s.Gender).HasConversion<string>().HasMaxLength(10).IsRequired();
            buildAction.Property(s => s.Status).HasConversion<string>().HasMaxLength(15).IsRequired();
            buildAction.Property(s => s.PlaceOfBirth).HasColumnType("varchar(100)").IsRequired();
            buildAction.Property(s => s.FatherName).HasColumnType("nvarchar(100)").IsRequired();
            buildAction.Property(s => s.MotherName).HasColumnType("nvarchar(100)").IsRequired();
            buildAction.Property(s => s.Contact).HasColumnType("varchar(100)").IsRequired();
            buildAction.Property(s => s.Address).HasColumnType("nvarchar(100)").IsRequired();
            buildAction.Property(s => s.BackgroundStudy).HasColumnType("nvarchar(100)").IsRequired();
            buildAction.HasIndex(s => s.Code).IsUnique().HasFilter("code <> ''");

            buildAction.HasMany(s => s.Registrations)
                        .WithOne(r => r.Student)
                        .HasForeignKey(s => s.StudentId)
                        .OnDelete(DeleteBehavior.Cascade);

            buildAction.HasMany(s => s.Payments)
                        .WithOne(r => r.Student)
                        .HasForeignKey(s => s.StudentId)
                        .OnDelete(DeleteBehavior.Cascade);

            buildAction.HasMany(s => s.Waitlists)
                        .WithOne(r => r.Student)
                        .HasForeignKey(s => s.StudentId)
                        .OnDelete(DeleteBehavior.Cascade);

            buildAction.ToTable("Students", t =>
            {

                t.HasCheckConstraint(
                    "CHK_FULLNAME_NOT_EMPTY",
                    "LEN(LTRIM(RTRIM([FullName]))) > 0"
                );

                t.HasCheckConstraint(
                    "CHK_LATINNAME_NOT_EMPTY",
                    "LEN(LTRIM(RTRIM([LatinName]))) > 0"
                );

                t.HasCheckConstraint(
                    "CHK_POB_NOT_EMPTY",
                    "LEN(LTRIM(RTRIM([PlaceOfBirth]))) > 0"
                );

                t.HasCheckConstraint(
                    "CHK_FATHERNAME_NOT_EMPTY",
                    "LEN(LTRIM(RTRIM([FatherName]))) > 0"
                );

                t.HasCheckConstraint(
                    "CHK_MOTHERNAME_NOT_EMPTY",
                    "LEN(LTRIM(RTRIM([MotherName]))) > 0"
                );

                t.HasCheckConstraint(
                    "CHK_CONTACT_NOT_EMPTY",
                    "LEN(LTRIM(RTRIM([Contact]))) > 0"
                );

                t.HasCheckConstraint(
                    "CHK_ADDRESS_NOT_EMPTY",
                    "LEN(LTRIM(RTRIM([Address]))) > 0"
                );

                t.HasCheckConstraint(
                    "CHK_BACKGROUNDSTUDY_NOT_EMPTY",
                    "LEN(LTRIM(RTRIM([BackgroundStudy]))) > 0"
                );
            });
        });
        #endregion
        #region Curriculum Bridges
        modelBuilder.Entity<Subject>(buildAction =>
        {
            buildAction.HasKey(s => s.Id);
            buildAction.Property(s => s.Id).HasColumnType("varchar(50)").IsRequired();
            buildAction.Property(s => s.Name).HasColumnType("nvarchar(100)").IsRequired();
            buildAction.Property(s => s.Code).HasColumnType("varchar(50)").IsRequired();

            buildAction.HasIndex(s => s.Name).IsUnique().HasFilter("[Name] <> ''");

            buildAction.HasMany(s => s.ClassSubjects)
                       .WithOne(cs => cs.Subject)
                       .HasForeignKey(cs => cs.SubjectId)
                       .OnDelete(DeleteBehavior.Cascade);

            buildAction.HasMany(s => s.TeacherSubjectClasses)
                        .WithOne(s => s.Subject)
                        .HasForeignKey(tsc => tsc.SubjectId)
                        .OnDelete(DeleteBehavior.Cascade);

            buildAction.HasMany(s =>s.Schedules)
                        .WithOne(s => s.Subject)
                        .HasForeignKey(tsc => tsc.SubjectId)
                        .OnDelete(DeleteBehavior.Cascade);

            buildAction.ToTable("Subjects", t =>
            {
                t.HasCheckConstraint(
                    "CHK_SUBJECT_NAME_NOT_EMPTY",
                    "LEN(LTRIM(RTRIM([Name]))) > 0"
                );

                t.HasCheckConstraint(
                    "CHK_SUBJECT_CODE_NOT_EMPTY",
                    "LEN(LTRIM(RTRIM([Code]))) > 0"
                );

            });
        });

        modelBuilder.Entity<ClassSubject>(buildAction =>
        {
            buildAction.HasKey(cs => cs.Id);
            buildAction.Property(cs => cs.Id).HasColumnType("varchar(50)").IsRequired();
            buildAction.Property(cs => cs.ClassId).HasColumnType("varchar(50)").IsRequired();
            buildAction.Property(cs => cs.SubjectId).HasColumnType("varchar(50)").IsRequired();
            //ClassSubject -> Class (Many-to-One)
            buildAction.HasOne(cs => cs.Class)
                        .WithMany(c => c.ClassSubjects)
                        .HasForeignKey(cs => cs.ClassId)
                        .OnDelete(DeleteBehavior.Cascade);
            //ClassSubject -> Subject (Many-to-One)
            buildAction.HasOne(cs => cs.Subject)
                        .WithMany(s => s.ClassSubjects)
                        .HasForeignKey(cs => cs.SubjectId)
                        .OnDelete(DeleteBehavior.Cascade);

            buildAction.ToTable("ClassSubjects");
        });

        modelBuilder.Entity<TeacherSubjectClass>(buildAction =>
        {
            buildAction.HasKey(tsc => tsc.Id);
            buildAction.Property(tsc => tsc.Id).HasColumnType("varchar(50)").IsRequired();
            buildAction.Property(tsc => tsc.TeacherId).HasColumnType("varchar(50)").IsRequired();
            buildAction.Property(tsc => tsc.SubjectId).HasColumnType("varchar(50)").IsRequired();
            buildAction.Property(tsc => tsc.ClassId).HasColumnType("varchar(50)").IsRequired();

            buildAction.HasOne(tsc => tsc.Subject)
                        .WithMany(s => s.TeacherSubjectClasses)
                        .HasForeignKey(tsc => tsc.SubjectId)
                        .OnDelete(DeleteBehavior.Cascade);

            buildAction.HasOne(tsc => tsc.Teacher)
                        .WithMany(t => t.TeacherSubjectClasses)
                        .HasForeignKey(tsc => tsc.TeacherId)
                        .OnDelete(DeleteBehavior.Cascade);

            buildAction.HasOne(tsc => tsc.Class)
                        .WithMany(c => c.TeacherSubjectClasses)
                        .HasForeignKey(tsc => tsc.ClassId)
                        .OnDelete(DeleteBehavior.Cascade);

            buildAction.ToTable("TeacherSubjectClasses");
        });
        #endregion
        #region Registrations
        modelBuilder.Entity<Registration>(buildAction =>
        {
            buildAction.HasKey(r => r.Id);
            buildAction.Property(r => r.Id).HasColumnType("varchar(50)").IsRequired();
            buildAction.Property(r => r.StudentId).HasColumnType("varchar(50)").IsRequired();
            buildAction.Property(r => r.ClassId).HasColumnType("varchar(50)").IsRequired();
            buildAction.Property(r => r.Status).HasConversion<string>().IsRequired();
            buildAction.Property(r => r.EnrolledBy).IsRequired(false);
            buildAction.Property(r => r.ProcessedBy).IsRequired(false);
            buildAction.Property(r => r.RejectedBy).IsRequired(false);
            buildAction.Property(r => r.Notes).HasColumnType("nvarchar(100)");
            buildAction.Property(r => r.CreatedAt).HasColumnType("date").IsRequired();
            buildAction.Property(r => r.RejectedAt).HasColumnType("datetime2").IsRequired(false);
            buildAction.Property(r => r.RejectionReason).HasColumnType("varchar(255)");

            // Registration → Student (Many-to-One)
            buildAction.HasOne(r => r.Student)
                       .WithMany(s => s.Registrations)
                       .HasForeignKey(r => r.StudentId)
                       .OnDelete(DeleteBehavior.Restrict);

            // Registration → Class (Many-to-One)
            buildAction.HasOne(r => r.Class)
                       .WithMany(c => c.Registrations)
                       .HasForeignKey(r => r.ClassId)
                       .OnDelete(DeleteBehavior.Restrict);

            buildAction.HasOne(r => r.EnrolledUser)
                       .WithMany()
                       .HasForeignKey(r => r.EnrolledBy)
                       .OnDelete(DeleteBehavior.Restrict);

            // Registration → User (ProcessedBy) (Many-to-One)
            buildAction.HasOne(r => r.ProcessedUser)
                       .WithMany(u => u.ProcessedRegistrations)
                       .HasForeignKey(r => r.ProcessedBy)
                       .OnDelete(DeleteBehavior.Restrict);

            buildAction.HasOne(r => r.RejectedUser)
                    .WithMany()
                    .HasForeignKey(r => r.RejectedBy)
                    .OnDelete(DeleteBehavior.Restrict);

            buildAction.ToTable("Registrations");
        });

        //modelBuilder.Entity<RegistrationStatus>(buildAction =>
        //{
        //    buildAction.HasKey(rs => rs.Id);
        //    buildAction.Property(rs => rs.Id).HasColumnType("varchar(50)").IsRequired();
        //    buildAction.Property(rs => rs.Name).HasColumnType("nvarchar(20)").IsRequired();

        //    buildAction.HasIndex(rs => rs.Name).IsUnique();

        //    buildAction.HasMany(rs => rs.Registrations)
        //               .WithOne(r => r.Status)
        //               .HasForeignKey(r => r.StatusId)
        //               .OnDelete(DeleteBehavior.Restrict);
        //    buildAction.ToTable("RegistrationStatuses");
        //});
        #endregion
        #region Payments & Waitlists
        modelBuilder.Entity<Payment>(buildAction =>
        {
            buildAction.HasKey(p => p.Id);
            buildAction.Property(p => p.Id).HasColumnType("varchar(50)").IsRequired();
            buildAction.Property(p => p.StudentId).HasColumnType("varchar(50)").IsRequired();
            buildAction.Property(p => p.Type).HasColumnType("nvarchar(20)").IsRequired();
            buildAction.Property(p => p.Method).HasColumnType("varchar(20)").IsRequired();
            buildAction.Property(p => p.Amount).HasColumnType("decimal(18,2)").IsRequired();
            buildAction.Property(p => p.ReferenceNumber).HasColumnType("varchar(50)").IsRequired(false);
            buildAction.Property(p => p.SlipURL).HasColumnType("varchar(255)").IsRequired(false);
            buildAction.Property(p => p.Status).HasColumnType("nvarchar(20)").IsRequired().HasDefaultValue("Pending");
            buildAction.Property(p => p.ReceivedBy).IsRequired();
            buildAction.Property(p => p.VerifiedBy).IsRequired(false);
            buildAction.Property(p => p.PaidAt).HasColumnType("datetime2").IsRequired(false);

            // Payment → Student (Many-to-One)
            buildAction.HasOne(p => p.Student)
                       .WithMany(s => s.Payments)
                       .HasForeignKey(p => p.StudentId)
                       .OnDelete(DeleteBehavior.Restrict);
            // Payment → User (ReceivedBy) (Many-to-One)
            buildAction.HasOne(p => p.ReceivedUser)
                       .WithMany(u => u.ReceivedPayments)
                       .HasForeignKey(p => p.ReceivedBy)
                       .OnDelete(DeleteBehavior.Restrict);
            // Payment → User (VerifiedBy) (Many-to-One)
            buildAction.HasOne(p => p.VerifiedUser)
                       .WithMany(u => u.VerifiedPayments)
                       .HasForeignKey(p => p.VerifiedBy)
                       .OnDelete(DeleteBehavior.Restrict);
            buildAction.ToTable("Payments", t =>
            {
                t.HasCheckConstraint("CHK_AMOUNT_POSITIVE", "[Amount] > 0");
                t.HasCheckConstraint("CHK_TYPE_NOT_EMPTY", "LEN(LTRIM(RTRIM([Type]))) > 0");
                t.HasCheckConstraint("CHK_METHOD_NOT_EMPTY", "LEN(LTRIM(RTRIM([Method]))) > 0");
            });
        });

        modelBuilder.Entity<Waitlist>(buildAction =>
        {
            buildAction.HasKey(w => w.Id);
            buildAction.Property(w => w.Id).HasColumnType("varchar(50)").IsRequired();
            buildAction.Property(w => w.StudentId).HasColumnType("varchar(50)").IsRequired();
            buildAction.Property(w => w.ClassId).HasColumnType("varchar(50)").IsRequired();
            buildAction.Property(w => w.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
            buildAction.Property(w => w.Position).IsRequired();
            buildAction.Property(w => w.Notes).HasColumnType("nvarchar(255)").IsRequired(false);
            buildAction.Property(w => w.RequestedAt).HasColumnType("datetime2").IsRequired();
            // Waitlist → Student (Many-to-One)
            buildAction.HasOne(w => w.Student)
                       .WithMany(s => s.Waitlists)
                       .HasForeignKey(w => w.StudentId)
                       .OnDelete(DeleteBehavior.Restrict);
            // Waitlist → Class (Many-to-One)
            buildAction.HasOne(w => w.Class)
                       .WithMany(c => c.Waitlists)
                       .HasForeignKey(w => w.ClassId)
                       .OnDelete(DeleteBehavior.Restrict);
            buildAction.ToTable("Waitlists");
        });
        #endregion
        #region Schedules
        modelBuilder.Entity<Schedule>(buildAction =>
        {
            buildAction.HasKey(s => s.Id);
            buildAction.Property(s => s.Id).HasColumnType("varchar(50)").IsRequired();
            buildAction.Property(s => s.ClassId).HasColumnType("varchar(50)").IsRequired();
            buildAction.Property(s => s.SubjectId).HasColumnType("varchar(50)").IsRequired();
            buildAction.Property(s => s.TeacherId).HasColumnType("varchar(50)").IsRequired();
            buildAction.Property(s => s.Day).HasColumnType("nvarchar(20)").IsRequired();
            buildAction.Property(s => s.StartTime).HasColumnType("time").IsRequired();
            buildAction.Property(s => s.EndTime).HasColumnType("time").IsRequired();

            buildAction.HasOne(s => s.Class)
                       .WithMany(c => c.Schedules)
                       .HasForeignKey(s => s.ClassId)
                       .OnDelete(DeleteBehavior.Cascade);
            buildAction.HasOne(s => s.Subject)
                       .WithMany(sub => sub.Schedules)
                       .HasForeignKey(s => s.SubjectId)
                       .OnDelete(DeleteBehavior.Cascade);
            buildAction.HasOne(s => s.Teacher)
                       .WithMany(t => t.Schedules)
                       .HasForeignKey(s => s.TeacherId)
                       .OnDelete(DeleteBehavior.Cascade);
            buildAction.ToTable("Schedules", t =>
            {
                t.HasCheckConstraint("CHK_DAYOFWEEK_NOT_EMPTY", "LEN(LTRIM(RTRIM([Day]))) > 0");
                t.HasCheckConstraint(
                    "CHK_STARTTIME_BEFORE_ENDTIME",
                    "[StartTime] < [EndTime]"
                );
            });
        });
        #endregion
        #region Enrollments
        modelBuilder.Entity<Enrollment>(buildAction =>
        {
            buildAction.HasKey(s => s.Id);

            buildAction.Property(s => s.Id).HasColumnType("varchar(50)").IsRequired();
            buildAction.Property( s => s.RegistrationId).HasColumnType("varchar(50)").IsRequired();
            buildAction.Property(s => s.StudentId).HasColumnType("varchar(50)").IsRequired();
            buildAction.Property(s => s.ClassId).HasColumnType("varchar(50)").IsRequired();
            buildAction.Property(s => s.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
            buildAction.Property(s => s.EnrolledAt).HasColumnType("datetime2").IsRequired();
            buildAction.Property(s => s.CompletedAt).HasColumnType("datetime2").IsRequired(false);
            buildAction.Property(s => s.DroppedAt).HasColumnType("datetime2").IsRequired(false);
            buildAction.Property(s => s.DropReason).HasColumnType("nvarchar(255)").IsRequired(false);

            buildAction.HasOne(e => e.Registration)
                       .WithMany()
                       .HasForeignKey(e => e.RegistrationId)
                       .OnDelete(DeleteBehavior.Restrict);

            buildAction.ToTable("Enrollments");
        });
        #endregion
    }
}
