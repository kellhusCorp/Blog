using Blog.Domain;
using Blog.Domain.Entities;
using Blog.PublicApi.Infrastructure.Paging;

namespace Blog.PublicApi.Models
{
    public class PostsIndexViewModel
    {
        public PostsIndexViewModel(
            PagedResult<Post> entries,
            List<Tag> tags,
            List<Post> popularPosts)
        {
            Entries = entries;
            Tags = tags;
            PopularPosts = popularPosts;
        }

        public PagedResult<Post> Entries { get; set; }

        public List<Tag> Tags { get; set; }

        public List<Post> PopularPosts { get; set; }

        public string? Search { get; set; }

        public string? Tag { get; set; }
    }
}