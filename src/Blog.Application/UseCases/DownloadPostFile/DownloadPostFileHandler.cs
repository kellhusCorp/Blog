using Blog.Application.Contexts;
using Blog.Application.Services;
using Blog.BLL.Providers;
using Blog.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.UseCases.DownloadPostFile
{
    public class DownloadPostFileHandler : IRequestHandler<DownloadPostFileQuery, Result<DownloadPostFileResponse>>
    {
        private const string DefaultContentType = "application/octet-stream";
        private readonly IDbContext _dbContext;
        private readonly PostsService _postsService;
        private readonly IBlogFileProvider _blogFileProvider;

        public DownloadPostFileHandler(IDbContext dbContext, IBlogFileProvider blogFileProvider, PostsService postsService)
        {
            _dbContext = dbContext;
            _postsService = postsService;
            _blogFileProvider = blogFileProvider;
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
            
            var fileBytes = await _blogFileProvider.GetFileAsync(postFile.Path);
            
            return Result<DownloadPostFileResponse>.Success(new DownloadPostFileResponse
            {
                FileName = postFile.Name,
                File = fileBytes,
                ContentType = DefaultContentType
            });
        }
    }
}