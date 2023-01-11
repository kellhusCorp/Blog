using MyBlogOnCore.BLL.Commands;
using MyBlogOnCore.DataSource.Contexts;

namespace MyBlogOnCore.BLL.Handlers;

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
        
        //TODO add notification
    }
}