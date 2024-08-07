using System.ComponentModel.DataAnnotations;
using Blog.Localization;

namespace Blog.Domain;

public class TagAssignment
{
    [Required(ErrorMessageResourceName = nameof(Resources.Validation_Required), ErrorMessageResourceType = typeof(Resources))]
    public Guid? BlogId { get; set; }

    public virtual Post? Blog { get; set; }

    [Required(ErrorMessageResourceName = nameof(Resources.Validation_Required), ErrorMessageResourceType = typeof(Resources))]
    public Guid? TagId { get; set; }

    public virtual Tag? Tag { get; set; }
}