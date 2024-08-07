using MediatR;

namespace Blog.BLL.Commands
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