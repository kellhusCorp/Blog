using Blog.BLL.Commands;
using Blog.Infrastructure.Contexts;

namespace Blog.BLL.Handlers;

public class AddCommentCommandHandler : ICommandHandler<AddCommentCommand>
{
    private readonly BlogDbContext context;

    public AddCommentCommandHandler(BlogDbContext context)
    {
        this.context = context;
    }

    public async Task ExecuteAsync(AddCommentCommand command)
    {
        context.Comments.Add(command.Comment);
        await context.SaveChangesAsync();
    }
}