using System.ComponentModel.DataAnnotations;
using Blog.Localization;

namespace MyBlogOnCore.Models;

public class BlogCommentViewModel
{
    public BlogCommentViewModel()
    {
    }

    public BlogCommentViewModel(string name, string comment)
    {
        Name = name;
        Comment = comment;
    }

    [StringLength(50, ErrorMessageResourceName = nameof(Resources.Validation_MaxLength), ErrorMessageResourceType = typeof(Resources))]
    [Required(ErrorMessageResourceName = nameof(Resources.Validation_Required), ErrorMessageResourceType = typeof(Resources))]
    [Display(Name = nameof(Resources.Name), ResourceType = typeof(Resources))]
    public string Name { get; set; }

    [StringLength(50, ErrorMessageResourceName = nameof(Resources.Validation_MaxLength), ErrorMessageResourceType = typeof(Resources))]
    [Display(Name = nameof(Resources.Email), ResourceType = typeof(Resources))]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    [StringLength(100, ErrorMessageResourceName = nameof(Resources.Validation_MaxLength), ErrorMessageResourceType = typeof(Resources))]
    [Display(Name = nameof(Resources.Homepage), ResourceType = typeof(Resources))]
    [DataType(DataType.Url)]
    public string? Homepage { get; set; }

    [Required(ErrorMessageResourceName = nameof(Resources.Validation_Required), ErrorMessageResourceType = typeof(Resources))]
    [Display(Name = nameof(Resources.Comment), ResourceType = typeof(Resources))]
    [DataType(DataType.MultilineText)]
    public string Comment { get; set; }
}