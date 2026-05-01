//using AutoMapper;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Models.People;
using SchoolAPI.Repositories;
using SchoolAPI.Repositories.School_Structures;
using SchoolAPI.Services.People;
using SchoolAPI.Services.School_Structures;
using System.Reflection;
using System.Text.Json.Serialization;

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
            //builder.Services.AddIdentity<User, Role>(options =>
            //{
            //    options.User.AllowedUserNameCharacters += " "; // add space
            //})
            //.AddEntityFrameworkStores<SchoolDbContext>()
            //.AddDefaultTokenProviders();

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

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();


            #region seeding roles, users
            // ---- Seed roles & users here ----
            bool.TryParse(builder.Configuration["IsDataSeeded"], out bool isDataSeeded);
            if (isDataSeeded)
            {
                using var scope = app.Services.CreateScope();
                await SchoolSeeder.SeedAsync(scope.ServiceProvider);
                await IdentitySeeder.SeedAsync(scope.ServiceProvider);
                await StudentSeeder.SeedAsync(scope.ServiceProvider);
            }
            #endregion

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
