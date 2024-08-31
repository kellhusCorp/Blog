using Blog.Domain.Entities;
using MediatR;

namespace Blog.BLL.Commands
{
    public class AddOrUpdateBlogCommand : IRequest
    {
        public Post Post { get; set; }
        public IEnumerable<string> Tags { get; set; }

        public AddOrUpdateBlogCommand(Post post, IEnumerable<string> tags)
        {
            Post = post;
            Tags = tags;
        }
    }
}