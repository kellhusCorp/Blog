using System.ComponentModel.DataAnnotations;
using MyBlogOnCore.Localization;

namespace MyBlogOnCore.Domain;

public abstract class BaseEntity
{
    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedOn = DateTimeOffset.UtcNow;
    }

    [Display(Name = nameof(Resources.Id), ResourceType = typeof(Resources))]
    public Guid Id { get; set; }
    
    [Display(Name = nameof(Resources.CreatedOn), ResourceType = typeof(Resources))]
    public DateTimeOffset CreatedOn { get; set; }
}