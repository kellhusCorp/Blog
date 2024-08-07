using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Blog.Localization;

namespace Blog.Domain;

public class PostFile : BaseEntity
{
    public PostFile(
        string name)
    {
        Name = name;
    }

    [Display(Name = nameof(Resources.Name), ResourceType = typeof(Resources))]
    [StringLength(100, ErrorMessageResourceName = nameof(Resources.Validation_MaxLength), ErrorMessageResourceType = typeof(Resources))]
    [Required(ErrorMessageResourceName = nameof(Resources.Validation_Required), ErrorMessageResourceType = typeof(Resources))]
    public string Name { get; set; }

    [Display(Name = nameof(Resources.Counter), ResourceType = typeof(Resources))]
    public int Counter { get; set; }

    [Required(ErrorMessageResourceName = nameof(Resources.Validation_Required), ErrorMessageResourceType = typeof(Resources))]
    public Guid? BlogId { get; set; }

    public virtual Post? Blog { get; set; }

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