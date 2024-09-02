using Blog.Application.Contexts;
using Blog.Application.Enums;
using Blog.Application.Factories;
using Blog.Application.Providers;
using Blog.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.UseCases.UpdatePostFile
{
    public class UpdatePostFileHandler : IRequestHandler<UpdatePostFileCommand, Result<Unit>>
    {
        private readonly IDbContext _context;
        private readonly IFileProviderFactory _fileProviderFactory;

        public UpdatePostFileHandler(
            IDbContext context,
            IFileProviderFactory fileProviderFactory)
        {
            _context = context;
            _fileProviderFactory = fileProviderFactory;
        }

        public async Task<Result<Unit>> Handle(UpdatePostFileCommand request, CancellationToken cancellationToken)
        {
            string fileName = request.FileName.Replace('/', '\\');
            fileName = fileName.Substring(fileName.IndexOf('\\') + 1);

            string extension = fileName.Substring(fileName.LastIndexOf('.') + 1);

            PostFile? blogFile = await _context.PostFiles
                .FirstOrDefaultAsync(f => f.BlogId == request.BlogId && f.Name == fileName, cancellationToken);

            if (blogFile == null)
            {
                blogFile = new PostFile(fileName)
                {
                    BlogId = request.BlogId,
                    Name = fileName
                };

                await _context.PostFiles.AddAsync(blogFile, cancellationToken);
            }

            IFileProvider provider = _fileProviderFactory.GetFileProvider(FileProviderType.File);

            await provider.AddFileAsync($"{blogFile.Id}.{extension}", request.Data);

            await _context.SaveChangesAsync(cancellationToken);

            return Result<Unit>.Success(Unit.Value);
        }
    }
}