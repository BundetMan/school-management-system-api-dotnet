using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.Models.Registrations
{
    public class RegistrationStatus
    {
        [Key]
        public string StatusId { get; set; } = default!;

        [Required, MaxLength(20)]
        public string Name { get; set; } = default!;

        public ICollection<Registration> Registrations { get; set; } = default!;
    }
}
