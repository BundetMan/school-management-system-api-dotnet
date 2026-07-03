using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SchoolAPI.Data;
using SchoolAPI.Middlewares;
using SchoolAPI.Models.People;
using SchoolAPI.Repositories;
using SchoolAPI.Repositories.Enrollments;
using SchoolAPI.Repositories.Registrations;
using SchoolAPI.Repositories.School_Structures;
using SchoolAPI.Repositories.Subjects;
using SchoolAPI.Repositories.Waitlists;
using SchoolAPI.Services;
using SchoolAPI.Services.Enrollments;
using SchoolAPI.Services.People;
using SchoolAPI.Services.Registrations;
using SchoolAPI.Services.School_Structures;
using SchoolAPI.Services.Waitlists;
using SchoolAPI.Services.Subjects;
using System.Reflection;
using System.Text.Json.Serialization;
using SchoolAPI.Repositories.People;
using SchoolAPI.Repositories.ClassSubjects;
using SchoolAPI.Services.ClassSubjects;
using SchoolAPI.Repositories.TeacherSubjectClasses;
using SchoolAPI.Services.TeacherSubjectClasses;
using SchoolAPI.Repositories.Schedules;
using SchoolAPI.Services.Schedules;
using SchoolAPI.Repositories.Payments;
using SchoolAPI.Services.Payments;


namespace SchoolAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                })
            ;

            builder.Services.AddDbContext<SchoolDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SchoolDatabase")));

            

            #region mapper Configuration
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly()); // picks up all IRegister classes

            builder.Services.AddSingleton(config);
            builder.Services.AddScoped<IMapper, ServiceMapper>();
            #endregion

            #region Dependency Injection for Repositories and Services
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<ISchoolLevelRepository, SchoolLevelRepository>();
            builder.Services.AddScoped<ISchoolLevelService, SchoolLevelService>();
            builder.Services.AddScoped<ILevelRepository, LevelRepository>();
            builder.Services.AddScoped<ILevelService, LevelService>();
            builder.Services.AddScoped<IClassRepository, ClassRepository>();
            builder.Services.AddScoped<IClassService, ClassService>();
            builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();
            builder.Services.AddScoped<IRegistrationService, RegistrationService>();
            builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
            builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
            builder.Services.AddScoped<IWaitlistRepository, WaitlistRepository>();
            builder.Services.AddScoped<IWaitlistService, WaitlistService>();
            builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
            builder.Services.AddScoped<ISubjectService, SubjectService>();
            builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
            builder.Services.AddScoped<ITeacherService, TeacherService>();
            builder.Services.AddScoped<IClassSubjectRepository, ClassSubjectRepository>();
            builder.Services.AddScoped<IClassSubjectService, ClassSubjectService>();
            builder.Services.AddScoped<ITeacherSubjectClassRepository, TeacherSubjectClassRepository>();
            builder.Services.AddScoped<ITeacherSubjectClassService, TeacherSubjectClassService>();
            builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();
            builder.Services.AddScoped<IScheduleService, ScheduleService>();
            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();

            builder.Services.AddScoped<ITokenService, TokenService>();

            #endregion

            #region Identity (Users + Roles with EF stores)
            builder.Services.AddIdentity<User, Role>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 8;
                opt.User.AllowedUserNameCharacters += " "; // add space
            })
            .AddEntityFrameworkStores<SchoolDbContext>()
            .AddDefaultTokenProviders();
            #endregion

            #region Authentication configuration

            var JwtKey = builder.Configuration["Jwt:Key"]!;
            var jwtIssuer = builder.Configuration["Jwt:Issuer"];
            var jwtAudience = builder.Configuration["Jwt:Audience"];

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = !string.IsNullOrWhiteSpace(jwtIssuer),
                        ValidateAudience = !string.IsNullOrWhiteSpace(jwtAudience),
                        ValidateIssuerSigningKey = true,
                        RequireExpirationTime = true,
                        ValidIssuer = jwtIssuer,
                        ValidAudience = jwtAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(JwtKey)),
                    };
                });


            #endregion

            #region configuration authorization policies
            builder.Services.AddAuthorization(options =>
            {
                //options.FallbackPolicy = new AuthorizationPolicyBuilder()
                //    .RequireAuthenticatedUser()
                //    .Build();
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
                options.AddPolicy("RequireTeacherRole", policy => policy.RequireRole("Teacher"));
                options.AddPolicy("RequireStudentRole", policy => policy.RequireRole("Student"));
            });

            #endregion

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            #region Global error handling middleware
            app.UseMiddleware<ErrorHandlingMiddleware>();
            #endregion

            #region seeder 
            // ---- Seed roles & users here ----
            bool.TryParse(builder.Configuration["IsDataSeeded"], out bool isDataSeeded);
            if (isDataSeeded)
            {
                using var scope = app.Services.CreateScope();
                await SchoolSeeder.SeedAsync(scope.ServiceProvider);
                await IdentitySeeder.SeedAsync(scope.ServiceProvider);
                await StudentSeeder.SeedAsync(scope.ServiceProvider);
                await RegistrationSeeder.SeedAsync(scope.ServiceProvider);
                await SubjectSeeder.SeedAsync(scope.ServiceProvider);
                await ClassSubjectSeeder.SeedAsync(scope.ServiceProvider);
                await TeacherSeeder.SeedAsync(scope.ServiceProvider);
                await TeacherSubjectClassSeeder.SeedAsync(scope.ServiceProvider);
            }
            #endregion

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
