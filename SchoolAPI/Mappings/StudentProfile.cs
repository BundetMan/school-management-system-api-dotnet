
using Mapster;
using SchoolAPI.DTOs.People;
using SchoolAPI.Models;
using SchoolAPI.Models.PaymentsWaitlists;
using SchoolAPI.Models.People;
using SchoolAPI.Models.Registrations;
namespace SchoolAPI.Mappings;
public class StudentProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Student, StudentDto>()
            .Map(dest => dest.LevelName, src => src.Level != null ? src.Level.Name : string.Empty)
            .Map(dest => dest.ClassName, src => src.Class != null ? src.Class.Name : string.Empty)
            .Map(dest => dest.Gender, src => src.Gender)
            .Map(dest => dest.Status, src => src.Status);

        config.NewConfig<Student, StudentDetailDto>()
            .Map(dest => dest.LevelName, src => src.Level != null ? src.Level.Name : string.Empty)
            .Map(dest => dest.ClassName, src => src.Class != null ? src.Class.Name : string.Empty)
            .Map(dest => dest.Gender, src => src.Gender.ToString())
            .Map(dest => dest.Status, src => src.Status.ToString());

        config.NewConfig<StudentCreateDto, Student>()
            .Map(dest => dest.Id, src => Guid.NewGuid().ToString())
            .Map(dest => dest.Code, src => $"ST-{Guid.NewGuid().ToString().Substring(0, 8)}")
            .Ignore(dest => dest.Level)
            .Ignore(dest => dest.Class)
            .Ignore(dest => dest.User)
            .Ignore(dest => dest.Registrations)
            .Ignore(dest => dest.Payments)
            .Ignore(dest => dest.Waitlists);

        config.NewConfig<StudentUpdateDetailDto, Student>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.Code)
            .Ignore(dest => dest.Level)
            .Ignore(dest => dest.Class)
            .Ignore(dest => dest.User)
            .Ignore(dest => dest.Registrations)
            .Ignore(dest => dest.Payments)
            .Ignore(dest => dest.Waitlists);

        config.NewConfig<StudentUpdateDto, Student>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.Code)
            .Ignore(dest => dest.Level)
            .Ignore(dest => dest.Class)
            .Ignore(dest => dest.User)
            .Ignore(dest => dest.Registrations)
            .Ignore(dest => dest.Payments)
            .Ignore(dest => dest.Waitlists)
            .IgnoreNullValues(true);

        config.NewConfig<Payment, PaymentSummaryDto>();

        config.NewConfig<Registration, RegistrationSummaryDto>()
            .Map(dest => dest.ClassName, src => src.Class.Name);

        config.NewConfig<Waitlist, WaitlistSummaryDto>()
            .Map(dest => dest.ClassName, src => src.Class.Name);
    }
}
