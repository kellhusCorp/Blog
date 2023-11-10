using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBlogOnCore.BLL.Commands;
using MyBlogOnCore.BLL.Exceptions;
using MyBlogOnCore.DataSource.Contexts;

namespace MyBlogOnCore.BLL.Handlers
{
    public class DeleteCommentHandler : IRequestHandler<DeleteCommentCommand>
    {
        private readonly BlogDbContext context;

        public DeleteCommentHandler(BlogDbContext context)
        {
            this.context = context;
        }

        public async Task Handle(DeleteCommentCommand request, CancellationToken cancellationToken = default)
        {
            var comment = await context.Comments
                .SingleOrDefaultAsync(c => c.Id == request.CommentId, cancellationToken: cancellationToken);

            if (comment == null)
            {
                throw new EntityNotFoundException($"Comment with id {request.CommentId} not found");
            }

            context.Comments.Remove(comment);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}