using Blog.Domain;
using Blog.PublicApi.Infrastructure.Paging;

namespace Blog.PublicApi.Models;

public class IndexViewModel
{
    public string? SearchTerm { get; set; }

    public PagedResult<Post>? Posts { get; set; }
}