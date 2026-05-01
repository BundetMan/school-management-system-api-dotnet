
using Mapster;
using SchoolAPI.DTOs.School_Structures;
using SchoolAPI.Models.School_Structure;

namespace SchoolAPI.Mappings
{
    public class LevelProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // Level
            config.NewConfig<Level, LevelDto>()
                .Map(dest => dest.SchoolLevelName, src => src.SchoolLevel.Name);

            config.NewConfig<LevelCreateDto, Level>()
                .Map(dest => dest.Id, src => Guid.NewGuid().ToString())
                .Map(dest => dest.SchoolLevelId, src => src.SchoolLevelId);

            config.NewConfig<LevelUpdateDto, Level>();
        }
    }
}
