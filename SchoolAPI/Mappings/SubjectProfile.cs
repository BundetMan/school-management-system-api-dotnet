using Mapster;
using SchoolAPI.DTOs.ClassSubject;
using SchoolAPI.DTOs.Subject;
using SchoolAPI.Models.SubjectAndBridge;

namespace SchoolAPI.Mappings
{
    public class SubjectProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Subject, SubjectDto>();
            config.NewConfig<Subject, SubjectDetailsDto>()
                .Map(dest => dest.ClassIds, src => src.ClassSubjects != null
                    ? src.ClassSubjects.Select(cs => cs.ClassId).ToList() : new List<string>())
                .Map(dest => dest.TeacherIds, src => src.TeacherSubjectClasses != null
                    ? src.TeacherSubjectClasses.Select(tsc => tsc.TeacherId).ToList() : new List<string>())
                .Map(dest => dest.ScheduleIds, src => src.Schedules != null
                    ? src.Schedules.Select(s => s.Id).ToList() : new List<string>());

            config.NewConfig<SubjectCreateDto, Subject>()
                .Map(dest => dest.Id, src => Guid.NewGuid().ToString());
            config.NewConfig<SubjectUpdateDto, Subject>();

            config.NewConfig<ClassSubject, ClassSubjectResponseDto>()
                .Map(desc => desc.Id, src => Guid.NewGuid().ToString())
                .Map(dest => dest.SubjectName, src => src.Subject != null ? src.Subject.Name : null)
                .Map(dest => dest.SubjectCode, src => src.Subject != null ? src.Subject.Code : null);
        }
    }
}
