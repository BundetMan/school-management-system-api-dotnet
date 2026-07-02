namespace SchoolAPI.DTOs.ClassSubject
{
    public class ClassSubjectsCreateDto
    {
        public string ClassId { get; set; } = default!;
        public List<string> SubjectIds { get; set; } = [];
    }
}
