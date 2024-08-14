using Blog.Application.Contexts;
using Blog.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.UseCases.DeletePostComment
{
    public class DeleteCommentHandler : IRequestHandler<DeleteCommentCommand, Result<DeleteCommentResponse>>
    {
        private readonly IDbContext context;

        public DeleteCommentHandler(IDbContext context)
        {
            this.context = context;
        }

        public async Task<Result<DeleteCommentResponse>> Handle(DeleteCommentCommand command, CancellationToken cancellationToken = default)
        {
            var postComment = await context.Comments
                .FirstOrDefaultAsync(c => c.Id == command.CommentId, cancellationToken);

            if (postComment is null)
            {
                return Result<DeleteCommentResponse>.Failure("Comment not found.");
            }

            context.Comments.Remove(postComment);
            await context.SaveChangesAsync(cancellationToken);
            return Result<DeleteCommentResponse>.Success(new DeleteCommentResponse());
        }
    }
}