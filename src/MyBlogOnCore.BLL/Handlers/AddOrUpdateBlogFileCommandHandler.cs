using Microsoft.EntityFrameworkCore;
using MyBlogOnCore.BLL.Commands;
using MyBlogOnCore.BLL.Providers;
using MyBlogOnCore.DataSource.Contexts;
using MyBlogOnCore.Domain;

namespace MyBlogOnCore.BLL.Handlers;

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
        
        BlogFile? blogFile = await context.Files
            .SingleOrDefaultAsync(f => f.BlogId == command.BlogId && f.Name == fileName);
        
        if (blogFile == null)
        {
            blogFile = new BlogFile(fileName)
            {
                BlogId = command.BlogId,
                Name = fileName
            };

            await context.Files.AddAsync(blogFile);
        }
        
        await fileProvider.AddFileAsync($"{blogFile.Id}.{extension}", command.Data);

        await context.SaveChangesAsync();
    }
}