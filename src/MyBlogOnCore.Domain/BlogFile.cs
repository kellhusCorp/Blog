using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyBlogOnCore.Localization;

namespace MyBlogOnCore.Domain;

public class BlogFile : BaseEntity
{
    public BlogFile(
        string name)
    {
        this.Name = name;
    }

    [Display(Name = nameof(Resources.Name), ResourceType = typeof(Resources))]
    [StringLength(100, ErrorMessageResourceName = nameof(Resources.Validation_MaxLength), ErrorMessageResourceType = typeof(Resources))]
    [Required(ErrorMessageResourceName = nameof(Resources.Validation_Required), ErrorMessageResourceType = typeof(Resources))]
    public string Name { get; set; }

    [Display(Name = nameof(Resources.Counter), ResourceType = typeof(Resources))]
    public int Counter { get; set; }

    [Required(ErrorMessageResourceName = nameof(Resources.Validation_Required), ErrorMessageResourceType = typeof(Resources))]
    public Guid? BlogId { get; set; }

    public virtual Blog? Blog { get; set; }

    [NotMapped]
    public string Path
    {
        get
        {
            var extension = Name.Substring(Name.LastIndexOf('.') + 1);
            return $"{Id}.{extension}";
        }
    }
}