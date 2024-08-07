using Blog.BLL.Commands;
using Blog.BLL.Providers;
using Blog.Domain;
using Blog.Infrastructure.Contexts;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Blog.BLL.Handlers;

[UsedImplicitly]
public class AddOrUpdateBlogFileCommandHandler : ICommandHandler<AddOrUpdateBlogFileCommand>
{
    private readonly BlogDbContext context;
    private readonly IBlogFileProvider fileProvider;

    public AddOrUpdateBlogFileCommandHandler(
        BlogDbContext context,
        IBlogFileProvider fileProvider)
    {
        this.context = context;
        this.fileProvider = fileProvider;
    }
    
    public async Task ExecuteAsync(AddOrUpdateBlogFileCommand command)
    {
        var fileName = command.FileName.Replace('/', '\\');
        fileName = fileName.Substring(fileName.IndexOf('\\') + 1);

        var extension = fileName.Substring(fileName.LastIndexOf('.') + 1);
        
        PostFile? blogFile = await context.PostFiles
            .SingleOrDefaultAsync(f => f.BlogId == command.BlogId && f.Name == fileName);
        
        if (blogFile == null)
        {
            blogFile = new PostFile(fileName)
            {
                BlogId = command.BlogId,
                Name = fileName
            };

            await context.PostFiles.AddAsync(blogFile);
        }
        
        await fileProvider.AddFileAsync($"{blogFile.Id}.{extension}", command.Data);

        await context.SaveChangesAsync();
    }
}