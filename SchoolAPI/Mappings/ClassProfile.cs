
using SchoolAPI.DTOs.School_Structures;
using SchoolAPI.Models.School_Structure;
using SchoolAPI.Models.People;
using Mapster;

namespace SchoolAPI.Mappings
{
    public class ClassProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Class, ClassDto>()
                .Map(dest => dest.LevelName, src => src.Level != null ? src.Level.Name : string.Empty)
                .Map(dest => dest.Status, src => src.Status);

            config.NewConfig<ClassCreateDto, Class>()
                .Map(dest => dest.Id, src => Guid.NewGuid().ToString())
                .Map(dest => dest.LevelId, src => src.LevelId)
                .Map(dest => dest.Status, src => ClassStatus.Active);

            config.NewConfig<ClassUpdateDto, Class>()
                .Ignore(dest => dest.Id)
                .Map(dest => dest.LevelId, src => src.LevelId)
                .Map(dest => dest.Status, src => src.Status);
        }
    }
}
