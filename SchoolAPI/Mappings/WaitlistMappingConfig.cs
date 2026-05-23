using Mapster;
using SchoolAPI.DTOs.Waitlist;
using SchoolAPI.Models.PaymentsWaitlists;

namespace SchoolAPI.Mappings
{
    public class WaitlistMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {

            config.NewConfig<Waitlist, WaitlistDto>();
            
            config.NewConfig<WaitlistRequestDto, Waitlist>()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.Position)
                .Ignore(dest => dest.RequestedAt)
                .Ignore(dest => dest.Student)
                .Ignore(dest => dest.Class);
        }
    }
}
