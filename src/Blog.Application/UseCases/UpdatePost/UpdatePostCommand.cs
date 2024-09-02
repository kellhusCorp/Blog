using Blog.Domain.Entities;
using MediatR;

namespace Blog.Application.UseCases.UpdatePost
{
    public class UpdatePostCommand : IRequest<Result<Unit>>
    {
        public Post Post { get; set; }
        public IEnumerable<string> Tags { get; set; }

        public UpdatePostCommand(Post post, IEnumerable<string> tags)
        {
            Post = post;
            Tags = tags;
        }
    }
}