using MediatR;

namespace MyBlogOnCore.BLL.Commands
{
    public class IncrementVisitsNumberCommand : IRequest
    {
        public Guid BlogId { get; set; }

        public IncrementVisitsNumberCommand(Guid blogId)
        {
            BlogId = blogId;
        }
    }
}