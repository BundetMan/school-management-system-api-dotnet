using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.Models.School_Structure
{
    public class SchoolLevel
    {
        public string Id { get; set; } = default!;//pk

        public string Name { get; set; } = string.Empty;

        public ICollection<Level> Levels { get; set; } = default!;
    }
}
