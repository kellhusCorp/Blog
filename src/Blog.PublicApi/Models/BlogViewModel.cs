using MyBlogOnCore.Domain;

namespace MyBlogOnCore.Models;

public class BlogViewModel
{
    public Blog BlogEntry { get; set; } = null!;

    public List<Blog> RelatedBlogEntries { get; set; } = null!;

    public BlogCommentViewModel Comment { get; set; } = null!;
}