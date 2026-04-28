namespace SchoolAPI.DTOs.People
{
    public class UserDto
    {
        public string Id { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public IEnumerable<string> Roles { get; set; } = new List<string>();
    }
}
