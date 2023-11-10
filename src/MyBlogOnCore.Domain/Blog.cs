using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyBlogOnCore.Localization;

namespace MyBlogOnCore.Domain;

public class Blog : BaseEntity
{
    public Blog()
    {
    }

    public Blog(
        string header,
        string permanentLink,
        string shortContent)
    {
        Header = header;
        PermanentLink = permanentLink;
        ShortContent = shortContent;
    }
    
    [Display(Name = nameof(Resources.BlogHeader), ResourceType = typeof(Resources))]
    public string Header { get; set; }
    
    public string PermanentLink { get; set; }
    
    [Display(Name = nameof(Resources.BlogShortContent), ResourceType = typeof(Resources))]
    public string ShortContent { get; set; }
    
    public string? Body { get; set; }
    
    [Display(Name = nameof(Resources.BlogIsVisible), ResourceType = typeof(Resources))]
    public bool IsVisible { get; set; }
    
    [Display(Name = nameof(Resources.BlogPublishDate), ResourceType = typeof(Resources))]
    public DateTimeOffset PublishDate { get; set; }
    
    public DateTimeOffset UpdateDate { get; set; }
    
    public int VisitsNumber { get; set; }
    
    public string? AuthorId { get; set; }
    
    public User? Author { get; set; }
    
    public virtual ICollection<TagAssignment>? TagAssignments { get; set; }
    
    public virtual ICollection<Comment>? Comments { get; set; }
    
    public virtual ICollection<BlogFile>? Files { get; set; }
    
    [NotMapped]
    public string Url => $"{PublishDate.Year}/{PublishDate.Month}/{PublishDate.Day}/{PermanentLink}";
}