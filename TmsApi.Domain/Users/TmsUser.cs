using Microsoft.AspNetCore.Identity;

namespace TmsApi.Domain.Users;

public class TmsUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string? Department { get; set; }
}