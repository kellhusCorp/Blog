using Blog.Domain;
using Blog.Domain.Entities;
using MediatR;

namespace Blog.Application.UseCases.DeletePostComment
{
    public class DeleteCommentCommand : IRequest<Result<DeleteCommentResponse>>
    {
        public Guid CommentId { get; set; }

        public DeleteCommentCommand(Guid commentId)
        {
            CommentId = commentId;
        }
    }
}