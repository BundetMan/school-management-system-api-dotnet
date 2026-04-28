using AutoMapper;
using SchoolAPI.DTOs.School_Structures;
using SchoolAPI.Models.School_Structure;
using SchoolAPI.Models.People;

namespace SchoolAPI.Mappings
{
    public class ClassProfile : Profile
    {
        public ClassProfile()
        {
            CreateMap<Class, ClassDto>()
                .ForMember(dest => dest.LevelName,
                    opt => opt.MapFrom(src => src.Level != null ? src.Level.Name : string.Empty))
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status));
            CreateMap<ClassCreateDto, Class>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.LevelId, opt => opt.MapFrom(src => src.LevelId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ClassStatus.Active));

            CreateMap<ClassUpdateDto, Class>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.LevelId, opt => opt.MapFrom(src => src.LevelId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));


        }
    }
}
