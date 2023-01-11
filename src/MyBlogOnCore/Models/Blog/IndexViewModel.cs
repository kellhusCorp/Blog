using MyBlogOnCore.Domain;
using MyBlogOnCore.Infrastructure.Paging;

namespace MyBlogOnCore.Models;

public class BlogsIndexViewModel
{
    public BlogsIndexViewModel(
        PagedResult<Blog> entries,
        List<Tag> tags,
        List<Domain.Blog> popularBlogEntries)
    {
        this.Entries = entries;
        this.Tags = tags;
        this.PopularBlogEntries = popularBlogEntries;
    }

    public PagedResult<Blog> Entries { get; set; }

    public List<Tag> Tags { get; set; }

    public List<Blog> PopularBlogEntries { get; set; }

    public string? Search { get; set; }

    public string? Tag { get; set; }
}
