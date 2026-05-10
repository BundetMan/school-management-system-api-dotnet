using Mapster;
using SchoolAPI.DTOs.Registration;
using SchoolAPI.Models.Registrations;

namespace SchoolAPI.Mappings
{
    public class RegistrationProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Registration, RegistrationDto>()
                .Map(dest => dest.StudentName, src => src.Student != null
                    ? src.Student.FullName
                    : string.Empty)
                .Map(dest => dest.ClassName, src => src.Class != null
                    ? src.Class.Name
                    : string.Empty);

            config.NewConfig<RegistrationCreateDto, Registration>()
                .Map(dest => dest.Id, _ => Guid.NewGuid().ToString())
                .Map(dest => dest.Status, _ => RegistrationStatus.Pending)
                .Map(dest => dest.CreatedAt, _ => DateTime.UtcNow)
                .Ignore(dest => dest.Student)
                .Ignore(dest => dest.Class)
                .Ignore(dest => dest.ProcessedUser)
                .Ignore(dest => dest.RejectedUser)
                .Ignore(dest => dest.ProcessedBy)
                .Ignore(dest => dest.ProcessedAt)
                .Ignore(dest => dest.RejectedBy)
                .Ignore(dest => dest.RejectedAt)
                .Ignore(dest => dest.RejectionReason);

            config.NewConfig<RegistrationApproveDto, Registration>()
                .Map(dest => dest.Status, _ => RegistrationStatus.Approved)
                .Map(dest => dest.ProcessedAt, _ => DateTime.UtcNow)
                .IgnoreNonMapped(true);

            config.NewConfig<RegistrationRejectDto, Registration>()
                .Map(dest => dest.Status, _ => RegistrationStatus.Rejected)
                .Map(dest => dest.RejectedAt, _ => DateTime.UtcNow)
                .IgnoreNonMapped(true);
        }
    }
}
