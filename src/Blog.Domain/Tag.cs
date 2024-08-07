using System.ComponentModel.DataAnnotations;
using Blog.Localization;

namespace Blog.Domain;

public class Tag : BaseEntity
{
    public Tag(
        string name)
    {
        this.Name = name;
    }

    [StringLength(30, ErrorMessageResourceName = nameof(Resources.Validation_MaxLength), ErrorMessageResourceType = typeof(Resources))]
    [Required(ErrorMessageResourceName = nameof(Resources.Validation_Required), ErrorMessageResourceType = typeof(Resources))]
    public string Name { get; set; }

    public virtual ICollection<TagAssignment>? TagAssignments { get; set; }
}