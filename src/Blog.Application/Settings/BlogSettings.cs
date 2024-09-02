namespace Blog.Application.Settings;

public class BlogSettings
{
    public string BlogName { get; set; } = null!;

    public string BlogDescription { get; set; } = null!;
    
    public bool NewUsersCanRegister { get; set; }
}