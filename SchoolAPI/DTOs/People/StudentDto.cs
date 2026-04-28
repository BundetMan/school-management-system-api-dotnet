using SchoolAPI.Models;
using SchoolAPI.Models.People;

namespace SchoolAPI.DTOs.People
{
    public record StudentDto(
        string Code,
        string FullName,
        string LatinName,
        GenderType Gender,
        StudentStatus Status,
        string LevelName,
        string ClassName
    );
}
