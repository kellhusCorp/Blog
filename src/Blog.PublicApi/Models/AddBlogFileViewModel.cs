using System.ComponentModel.DataAnnotations;
using Blog.Localization;

namespace Blog.PublicApi.Models;

public class AddBlogFileViewModel
{
    [Required(ErrorMessageResourceName = nameof(Resources.Validation_Required), ErrorMessageResourceType = typeof(Resources))]
    public Guid? BlogId { get; set; }

    [Required(ErrorMessageResourceName = nameof(Resources.Validation_Required), ErrorMessageResourceType = typeof(Resources))]
    public IFormFile? File { get; set; }
}