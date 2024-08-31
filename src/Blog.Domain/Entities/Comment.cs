using System.ComponentModel.DataAnnotations;
using Blog.Localization;

namespace Blog.Domain.Entities;

public class Comment : BaseEntity
{
    public Comment(string name, string body)
    {
        Name = name;
        Body = body;
    }

    [StringLength(50, ErrorMessageResourceName = nameof(Resources.Validation_MaxLength), ErrorMessageResourceType = typeof(Resources))]
    [Required(ErrorMessageResourceName = nameof(Resources.Validation_Required), ErrorMessageResourceType = typeof(Resources))]
    [Display(Name = nameof(Resources.Name), ResourceType = typeof(Resources))]
    public string Name { get; set; }

    [Required(ErrorMessageResourceName = nameof(Resources.Validation_Required), ErrorMessageResourceType = typeof(Resources))]
    [Display(Name = nameof(Resources.Comment), ResourceType = typeof(Resources))]
    public string Body { get; set; }

    [StringLength(50, ErrorMessageResourceName = nameof(Resources.Validation_MaxLength), ErrorMessageResourceType = typeof(Resources))]
    [Display(Name = nameof(Resources.Email), ResourceType = typeof(Resources))]
    public string? Email { get; set; }

    [StringLength(100, ErrorMessageResourceName = nameof(Resources.Validation_MaxLength), ErrorMessageResourceType = typeof(Resources))]
    [Display(Name = nameof(Resources.Homepage), ResourceType = typeof(Resources))]
    public string? Homepage { get; set; }

    public bool AdminPost { get; set; }

    [Required(ErrorMessageResourceName = nameof(Resources.Validation_Required), ErrorMessageResourceType = typeof(Resources))]
    public Guid? BlogId { get; set; }

    public virtual Post? Blog { get; set; }
}