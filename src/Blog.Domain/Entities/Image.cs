using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Blog.Localization;

namespace Blog.Domain.Entities;

public class Image : BaseEntity
{
    public Image(
        string name)
    {
        this.Name = name;
    }

    [StringLength(50, ErrorMessageResourceName = nameof(Resources.Validation_MaxLength), ErrorMessageResourceType = typeof(Resources))]
    [Required(ErrorMessageResourceName = nameof(Resources.Validation_Required), ErrorMessageResourceType = typeof(Resources))]
    public string Name { get; set; }

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