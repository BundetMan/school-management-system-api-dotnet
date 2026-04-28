using AutoMapper;
using SchoolAPI.DTOs.People;
using SchoolAPI.Models;
using SchoolAPI.Models.PaymentsWaitlists;
using SchoolAPI.Models.People;
using SchoolAPI.Models.Registrations;
namespace SchoolAPI.Mappings;
public class StudentProfile : Profile
{
    public StudentProfile()
    {
        CreateMap<Student, StudentDto>()
            .ForMember(dest => dest.LevelName, opt => opt.MapFrom(src => src.Level != null ? src.Level.Name : string.Empty))
            .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Class != null ? src.Class.Name : string.Empty))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

        CreateMap<Student, StudentDetailDto>()
            .ForMember(dest => dest.LevelName, opt => opt.MapFrom(src => src.Level != null ? src.Level.Name : string.Empty))
            .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Class != null ? src.Class.Name : string.Empty))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));


        CreateMap<StudentCreateDto, Student>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => $"ST-{Guid.NewGuid().ToString().Substring(0, 8)}"))
            .ForMember(dest => dest.Level, opt => opt.Ignore())
            .ForMember(dest => dest.Class, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.Registrations, opt => opt.Ignore())
            .ForMember(dest => dest.Payments, opt => opt.Ignore())
            .ForMember(dest => dest.Waitlists, opt => opt.Ignore());

        CreateMap<StudentUpdateDetailDto, Student>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Code, opt => opt.Ignore())
            .ForMember(dest => dest.Level, opt => opt.Ignore())
            .ForMember(dest => dest.Class, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.Registrations, opt => opt.Ignore())
            .ForMember(dest => dest.Payments, opt => opt.Ignore())
            .ForMember(dest => dest.Waitlists, opt => opt.Ignore());

        CreateMap<StudentUpdateDto, Student>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Code, opt => opt.Ignore())
            .ForMember(dest => dest.Level, opt => opt.Ignore())
            .ForMember(dest => dest.Class, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.Registrations, opt => opt.Ignore())
            .ForMember(dest => dest.Payments, opt => opt.Ignore())
            .ForMember(dest => dest.Waitlists, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


        CreateMap<Payment, PaymentSummaryDto>(); 
        CreateMap<Registration, RegistrationSummaryDto>().ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Class.Name)); 
        CreateMap<Waitlist, WaitlistSummaryDto>().ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Class.Name));
    }
}
