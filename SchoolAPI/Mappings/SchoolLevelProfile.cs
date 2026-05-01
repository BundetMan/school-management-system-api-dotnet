
using Mapster;
using SchoolAPI.DTOs.School_Structures;
using SchoolAPI.Models.School_Structure;

namespace SchoolAPI.Mappings
{
    public class SchoolLevelProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<SchoolLevel, SchoolLevelDto>();

            config.NewConfig<SchoolLevelCreateDto, SchoolLevel>()
                .Map(dest => dest.Id, src => Guid.NewGuid().ToString());

            config.NewConfig<SchoolLevelUpdateDto, SchoolLevel>();
        }
    }
}
