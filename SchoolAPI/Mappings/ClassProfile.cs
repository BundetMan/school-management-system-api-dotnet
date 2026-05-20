
using Mapster;
using SchoolAPI.DTOs.School_Structures;
using SchoolAPI.Models.People;
using SchoolAPI.Models.Registrations;
using SchoolAPI.Models.School_Structure;

namespace SchoolAPI.Mappings
{
    public class ClassProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Class, ClassDto>()
                .Map(dest => dest.LevelName, src => src.Level != null ? src.Level.Name : string.Empty)
                .Map(desc => desc.SchoolLevelName, src => src.Level != null && src.Level.SchoolLevel != null ? src.Level.SchoolLevel.Name : string.Empty)

                .Map(dest => dest.Status, src => src.Status)

                .Map(dest => dest.EnrolledCount, src => src.Registrations != null
                    ? src.Registrations.Count(r => r.Status == RegistrationStatus.Approved) : 0)
                .Map(dest => dest.AvailableSeats, src => src.Registrations != null
                    ? src.Capacity - src.Registrations.Count(r => r.Status == RegistrationStatus.Approved) : src.Capacity)
                .Map(dest => dest.IsFull, src => src.Registrations != null
                    && src.Registrations.Count(r => r.Status == RegistrationStatus.Approved) >= src.Capacity)

                .Map(dest => dest.SubjectCount, src => src.ClassSubjects != null
                    ? src.ClassSubjects.Count : 0)

                .Map(dest => dest.WaitlistCount, src => src.Waitlists != null
                    ? src.Waitlists.Count : 0);

            config.NewConfig<ClassCreateDto, Class>()
                .Map(dest => dest.Id, src => Guid.NewGuid().ToString())
                .Map(dest => dest.LevelId, src => src.LevelId)
                .Map(dest => dest.Status, src => ClassStatus.Active);

            config.NewConfig<ClassUpdateDto, Class>()
                .Ignore(dest => dest.Id)
                .Map(dest => dest.LevelId, src => src.LevelId)
                .Map(dest => dest.Status, src => src.Status);
        }
    }
}
