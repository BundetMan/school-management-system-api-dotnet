namespace SchoolAPI.DTOs.People
{
    public class UserWithRolesDto
    {
        public string? UserName { get; set; }
        public IList<string>? Roles { get; set; }
    }

}
