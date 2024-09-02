using System.ComponentModel.DataAnnotations;
using Blog.Domain;
using Blog.Domain.Entities;
using Blog.Localization;
using Blog.PublicApi.Infrastructure.Paging;

namespace Blog.PublicApi.Models;

public class ImagesViewModel
{
    public string? SearchTerm { get; set; }

    public PagedResult<Image>? Images { get; set; }

    [Required(ErrorMessageResourceName = nameof(Resources.Validation_Required), ErrorMessageResourceType = typeof(Resources))]
    public IFormFile? Image { get; set; }
}
