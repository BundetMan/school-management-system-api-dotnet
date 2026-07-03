using Mapster;
using SchoolAPI.DTOs.Payment;
using SchoolAPI.Models.PaymentsWaitlists;

namespace SchoolAPI.Mappings
{
    public class PaymentProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreateOfficePaymentDto, Payment>()
                .Map(dest => dest.Status, src => PaymentStatus.Paid)
                .Map(dest => dest.PaidAt, src => DateTime.UtcNow)
                .Ignore(dest => dest.Id);

            config.NewConfig<CreateOnlinePaymentDto, Payment>()
                .Map(dest => dest.Status, src => PaymentStatus.Pending)
                .Ignore(dest => dest.PaidAt) // set only on verification
                .Ignore(dest => dest.Id);

            config.NewConfig<Payment, PaymentResponseDto>()
                .Map(dest => dest.StudentName, src => src.Student.FullName)
                .Map(dest => dest.ReceivedByName, src => src.ReceivedUser.UserName)
                .Map(dest => dest.VerifiedByName, src => src.VerifiedUser != null ? src.VerifiedUser.UserName : null);
        }
}
}
