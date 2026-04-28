using AutoMapper;
using SchoolAPI.DTOs.School_Structures;
using SchoolAPI.Models.School_Structure;

namespace SchoolAPI.Mappings
{
    public class LevelProfile : Profile
    {
        public LevelProfile()
        {
            CreateMap<Level, LevelDto>()
                .ForMember(dest => dest.SchoolLevelName,
               opt => opt.MapFrom(src => src.SchoolLevel.Name));
            CreateMap<LevelCreateDto, Level>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.SchoolLevelId, opt => opt.MapFrom(src => src.SchoolLevelId));
            CreateMap<LevelUpdateDto, Level>();
        }
    }
}
