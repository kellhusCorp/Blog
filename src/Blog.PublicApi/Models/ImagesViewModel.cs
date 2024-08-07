using System.ComponentModel.DataAnnotations;
using Blog.Domain;
using Blog.Localization;
using MyBlogOnCore.Infrastructure.Paging;

namespace MyBlogOnCore.Models;

public class ImagesViewModel
{
    public string? SearchTerm { get; set; }

    public PagedResult<Image>? Images { get; set; }

    [Required(ErrorMessageResourceName = nameof(Resources.Validation_Required), ErrorMessageResourceType = typeof(Resources))]
    public IFormFile? Image { get; set; }
}
