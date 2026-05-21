using SchoolAPI.Models;
using SchoolAPI.Models.People;

namespace SchoolAPI.DTOs.People
{
    public record StudentDto(
        string Id,
        string Code,
        string FullName,
        string LatinName,
        GenderType Gender,
        StudentStatus Status
    );
}
