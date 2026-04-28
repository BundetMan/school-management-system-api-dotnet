using AutoMapper;
using SchoolAPI.DTOs.School_Structures;
using SchoolAPI.Models.School_Structure;

namespace SchoolAPI.Mappings
{
    public class SchoolLevelProfile : Profile
    {
        public SchoolLevelProfile()
        {
            CreateMap<SchoolLevel, SchoolLevelDto>();
            CreateMap<SchoolLevelCreateDto, SchoolLevel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()));

            CreateMap<SchoolLevelUpdateDto, SchoolLevel>();
        }
    }
}
