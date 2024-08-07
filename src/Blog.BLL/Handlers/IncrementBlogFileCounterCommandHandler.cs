using Blog.BLL.Commands;
using Blog.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Blog.BLL.Handlers;

public class IncrementBlogFileCounterCommandHandler : ICommandHandler<IncrementBlogFileCounterCommand>
{
    private readonly BlogDbContext context;

    public IncrementBlogFileCounterCommandHandler(BlogDbContext context)
    {
        this.context = context;
    }
    
    public async Task ExecuteAsync(IncrementBlogFileCounterCommand command)
    {
        await context.Database.ExecuteSqlInterpolatedAsync(
            $"UPDATE \"Files\" SET \"Counter\" = \"Counter\" + 1 WHERE \"Id\" = {command.Id}");
    }
}