using SchoolAPI.Models.School_Structure;

namespace SchoolAPI.DTOs.School_Structures
{
    public class ClassDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; } = 0;
        public ClassStatus Status { get; set; } = ClassStatus.Active;
        public string LevelId { get; set; } = string.Empty;
        public string LevelName { get; set; } = string.Empty;
        public string SchoolLevelName { get; set; } = default!;

        // Availability
        public int EnrolledCount { get; set; }
        public int AvailableSeats { get; set; }
        public bool IsFull { get; set; }

        // Counts
        public int SubjectCount { get; set; }
        public int WaitlistCount { get; set; }
    }
}
