using MyBlogOnCore.Domain;
using MyBlogOnCore.Infrastructure.Paging;

namespace MyBlogOnCore.Models;

public class IndexViewModel
{
    public string? SearchTerm { get; set; }

    public PagedResult<Blog>? Blogs { get; set; }
}