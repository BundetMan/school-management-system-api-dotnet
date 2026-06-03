namespace SchoolAPI.DTOs.TeacherSubjectClasses
{
    public class TeacherSubjectClassDto
    {
        public string Id { get; set; } = default!;
        public string TeacherId { get; set; } = default!;
        public string TeacherName { get; set; } = default!;
        public string ClassSubjectId { get; set; } = default!;
        public string ClassId { get; set; } = default!;
        public string ClassName { get; set; } = default!;
        public string SubjectId { get; set; } = default!;
        public string SubjectName { get; set; } = default!;
    }
    public class TeacherSubjectClassCreateDto
    {
        public string TeacherId { get; set; } = default!;
        public string ClassSubjectId { get; set; } = default!;
    }

    public class TeacherSubjectClassUpdateDto
    {
        public string TeacherId { get; set; } = default!;
        public string ClassSubjectId { get; set; } = default!;
    }
}
