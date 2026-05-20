using Mapster;
using SchoolAPI.DTOs.Enrollment;
using SchoolAPI.Models.Enrollment;

namespace SchoolAPI.Mappings
{
    public class EnrollmentProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Enrollment, EnrollmentDto>()
                .Map(dest => dest.Status, src => src.Status.ToString())
                .Map(dest => dest.EnrolledById, src => src.Registration.EnrolledBy)
                .Map(dest => dest.StudentName, src => src.Student.FullName)
                .Map(dest => dest.ClassName, src => src.Class.Name)
                .Map(dest => dest.EnrolledByName, src => src.Registration.EnrolledUser != null 
                            ? src.Registration.EnrolledUser.UserName : null);

        }
    }
}
