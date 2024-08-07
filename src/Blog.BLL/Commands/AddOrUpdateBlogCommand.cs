using MediatR;

namespace Blog.BLL.Commands
{
    public class AddOrUpdateBlogCommand : IRequest
    {
        public Domain.Post Post { get; set; }
        public IEnumerable<string> Tags { get; set; }

        public AddOrUpdateBlogCommand(Domain.Post post, IEnumerable<string> tags)
        {
            Post = post;
            Tags = tags;
        }
    }
}