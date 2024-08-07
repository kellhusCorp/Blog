namespace MyBlogOnCore.Domain;

public class Project
{
    public int ProjectId { get; set; }
    
    public string CategoryName { get; set; }
    
    public string? LinkToSite { get; set; }
    
    public string? LinkToImage { get; set; }
    
    public string Title { get; set; }
    
    public string Description { get; set; }
}