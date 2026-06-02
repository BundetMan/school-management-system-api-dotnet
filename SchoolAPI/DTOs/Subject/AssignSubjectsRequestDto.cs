namespace SchoolAPI.DTOs.Subject
{
    public class AssignSubjectsRequestDto
    {
        public string ClassId { get; set; } = default!;
        public List<string> SubjectIds { get; set; } = [];
    }
    public class ClassSubjectResponseDto
    {
        public string Id { get; set; } = default!;
        public string SubjectId { get; set; } = default!;
        public string SubjectName { get; set; } = default!;
        public string SubjectCode { get; set; } = default!;
    }
}
