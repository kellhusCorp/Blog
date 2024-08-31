using Blog.Application.Enums;
using Blog.Application.Factories;
using Blog.Application.Providers;
using Blog.BLL.Commands;
using Blog.Domain;
using Blog.Domain.Entities;
using Blog.Infrastructure.Contexts;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Blog.BLL.Handlers
{
    [UsedImplicitly]
    public class AddOrUpdateBlogFileCommandHandler : ICommandHandler<AddOrUpdateBlogFileCommand>
    {
        private readonly BlogDbContext _context;
        private readonly IFileProviderFactory _fileProviderFactory;

        public AddOrUpdateBlogFileCommandHandler(
            BlogDbContext context,
            IFileProviderFactory fileProviderFactory)
        {
            _context = context;
            _fileProviderFactory = fileProviderFactory;
        }

        public async Task ExecuteAsync(AddOrUpdateBlogFileCommand command)
        {
            string fileName = command.FileName.Replace('/', '\\');
            fileName = fileName.Substring(fileName.IndexOf('\\') + 1);

            string extension = fileName.Substring(fileName.LastIndexOf('.') + 1);

            PostFile? blogFile = await _context.PostFiles
                .SingleOrDefaultAsync(f => f.BlogId == command.BlogId && f.Name == fileName);

            if (blogFile == null)
            {
                blogFile = new PostFile(fileName)
                {
                    BlogId = command.BlogId,
                    Name = fileName
                };

                await _context.PostFiles.AddAsync(blogFile);
            }

            IFileProvider provider = _fileProviderFactory.GetFileProvider(FileProviderType.File);

            await provider.AddFileAsync($"{blogFile.Id}.{extension}", command.Data);

            await _context.SaveChangesAsync();
        }
    }
}