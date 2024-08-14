using Blog.Application.Dtos;

namespace Blog.Application.UseCases.GetPostByLink
{
    public class PostWithRelatedPostsDto
    {
        public PostDto Post { get; set; }
        
        public IEnumerable<PostDto> RelatedPosts { get; set; }
    }
}