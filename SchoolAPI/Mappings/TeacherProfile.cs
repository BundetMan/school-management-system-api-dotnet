using Mapster;
using SchoolAPI.DTOs.People;
using SchoolAPI.Models.People;

namespace SchoolAPI.Mappings
{
    public class TeacherProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Teacher, TeacherWithAssignmentsDto>()
            .Map(dest => dest.Assignments,
                 src => src.TeacherSubjectClasses.Adapt<IEnumerable<SubjectClassAssignmentDto>>());

            // Teacher → TeacherWithSchedulesDto
            config.NewConfig<Teacher, TeacherWithSchedulesDto>()
                .Map(dest => dest.Schedules,
                     src => src.Schedules.Adapt<IEnumerable<ScheduleSummaryDto>>());

            // Flat mappings are convention-based — Mapster handles these automatically
            config.NewConfig<Teacher, TeacherDto>();
            config.NewConfig<TeacherCreateDto, Teacher>()
                .Map(dest => dest.Id, _ => Guid.NewGuid().ToString())
                .Map(dest => dest.IsActive, _ => true);

            config.NewConfig<TeacherUpdateDto, Teacher>()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.UserId)
                .Ignore(dest => dest.User)
                .Ignore(dest => dest.TeacherSubjectClasses)
                .Ignore(dest => dest.Schedules);
        }
    }
}
