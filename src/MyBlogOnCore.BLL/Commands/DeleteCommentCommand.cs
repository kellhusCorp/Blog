using MediatR;

namespace MyBlogOnCore.BLL.Commands
{
    public class DeleteCommentCommand : IRequest
    {
        public Guid CommentId { get; set; }

        public DeleteCommentCommand(Guid commentId)
        {
            CommentId = commentId;
        }
    }
}