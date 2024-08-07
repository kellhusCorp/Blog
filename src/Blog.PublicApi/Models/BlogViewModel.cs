using Blog.Domain;
using MyBlogOnCore.Models;

namespace Blog.PublicApi.Models;

public class BlogViewModel
{
    public Post BlogEntry { get; set; } = null!;

    public List<Post> RelatedBlogEntries { get; set; } = null!;

    public BlogCommentViewModel Comment { get; set; } = null!;
}