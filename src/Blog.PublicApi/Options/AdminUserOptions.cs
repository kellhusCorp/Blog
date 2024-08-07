namespace MyBlogOnCore.Options;

public class AdminUserOptions
{
    public string FirstName { get; init; }
    
    public string LastName { get; init; }
    
    public string Email { get; init; }

    public string Password { get; init; }

    public bool IsConfirmed { get; init; }

    public string[] RoleNames { get; init; } = { "Admin" };
}