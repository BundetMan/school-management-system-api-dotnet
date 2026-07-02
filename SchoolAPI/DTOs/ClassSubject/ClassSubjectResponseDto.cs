namespace SchoolAPI.DTOs.ClassSubject
{
    public class ClassSubjectResponseDto
    {
        public string Id { get; set; } = default!;
        public string ClassId { get; set; } = default!;
        public string ClassName { get; set; } = default!;
        public string SubjectId { get; set; } = default!;
        public string SubjectName { get; set; } = default!;
        public string SubjectCode { get; set; } = default!;
    }
}
