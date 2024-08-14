using Blog.Application.Dtos;
using Blog.Application.UseCases.GetPostByLink;
using MyBlogOnCore.Models;

namespace Blog.PublicApi.Models
{
    public class PostViewModel
    {
        public PostDto Post { get; set; } = null!;

        public List<PostDto> RelatedPosts { get; set; } = null!;

        public BlogCommentViewModel Comment { get; set; } = null!;
    }
}