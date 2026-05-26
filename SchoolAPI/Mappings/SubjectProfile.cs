using Mapster;
using SchoolAPI.DTOs.Subject;
using SchoolAPI.Models.Curriculum_Bridges;

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
        }
    }
}
