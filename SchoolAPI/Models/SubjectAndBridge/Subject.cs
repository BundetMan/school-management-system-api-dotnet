using SchoolAPI.Models.Schedules;

namespace SchoolAPI.Models.SubjectAndBridge
{
    public class Subject
    {
        public string Id { get; set; } = String.Empty;

        public string Name { get; set; } = String.Empty;

        public string Code { get; set; } = String.Empty;

        public ICollection<ClassSubject> ClassSubjects { get; set; } = [];
        public ICollection<TeacherSubjectClass> TeacherSubjectClasses { get; set; } = [];
        public ICollection<Schedule> Schedules { get; set; } = [];
    }
}
