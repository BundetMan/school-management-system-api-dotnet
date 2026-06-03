using Mapster;
using SchoolAPI.DTOs.TeacherSubjectClasses;
using SchoolAPI.Models.SubjectAndBridge;

namespace SchoolAPI.Mappings
{
    public class TeacherSubjectClassProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<TeacherSubjectClass, TeacherSubjectClassDto>()
                .Map(dest => dest.TeacherName,
                     src => src.Teacher.Name)
                .Map(dest => dest.ClassId,
                     src => src.ClassSubject.ClassId)
                .Map(dest => dest.ClassName,
                     src => src.ClassSubject.Class.Name)
                .Map(dest => dest.SubjectId,
                     src => src.ClassSubject.SubjectId)
                .Map(dest => dest.SubjectName,
                     src => src.ClassSubject.Subject.Name);

            config.NewConfig<TeacherSubjectClassCreateDto, TeacherSubjectClass>();
            config.NewConfig<TeacherSubjectClassUpdateDto, TeacherSubjectClass>()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.Teacher)
                .Ignore(dest => dest.ClassSubject);
        }
    }
}
