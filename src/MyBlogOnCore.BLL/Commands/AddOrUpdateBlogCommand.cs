using MediatR;
using MyBlogOnCore.Domain;

namespace MyBlogOnCore.BLL.Commands
{
    public class AddOrUpdateBlogCommand : IRequest
    {
        public Blog Blog { get; set; }
        public IEnumerable<string> Tags { get; set; }

        public AddOrUpdateBlogCommand(Blog blog, IEnumerable<string> tags)
        {
            Blog = blog;
            Tags = tags;
        }
    }
}