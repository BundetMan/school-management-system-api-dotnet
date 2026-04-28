using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.Models.Registrations
{
    public class RegistrationStatus
    {
        public string Id { get; set; } = default!;

        [Required, MaxLength(20)]
        public string Name { get; set; } = default!;

        public ICollection<Registration> Registrations { get; set; } = default!;
    }
}
