using Blog.Application.Contexts;
using Blog.Application.Enums;
using Blog.Application.Factories;
using Blog.Application.Providers;
using Blog.Application.Services;
using Blog.Domain;
using Blog.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.UseCases.DownloadPostFile
{
    public class DownloadPostFileHandler : IRequestHandler<DownloadPostFileQuery, Result<DownloadPostFileResponse>>
    {
        private const string DefaultContentType = "application/octet-stream";
        private readonly IDbContext _dbContext;
        private readonly PostsService _postsService;
        private readonly IFileProviderFactory _fileProviderFactory;

        public DownloadPostFileHandler(IDbContext dbContext, IFileProviderFactory fileProviderFactory, PostsService postsService)
        {
            _dbContext = dbContext;
            _fileProviderFactory = fileProviderFactory;
            _postsService = postsService;
        }
        
        public async Task<Result<DownloadPostFileResponse>> Handle(DownloadPostFileQuery request, CancellationToken cancellationToken)
        {
            var postFile = await _dbContext.PostFiles
                .FirstOrDefaultAsync(x => x.Id == request.PostFileId, cancellationToken);
            
            if (postFile is null)
            {
                return Result<DownloadPostFileResponse>.Failure("Файл не найден");
            }
            
            _ = await _postsService.IncrementPostFileDownloadsNumber(request.PostFileId, cancellationToken);
            
            var provider = _fileProviderFactory.GetFileProvider(FileProviderType.File);
            
            var fileBytes = await provider.GetFileAsync(postFile.Path);
            
            return Result<DownloadPostFileResponse>.Success(new DownloadPostFileResponse
            {
                FileName = postFile.Name,
                File = fileBytes,
                ContentType = DefaultContentType
            });
        }
    }
}