using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blog.Application.Contexts;
using Blog.Application.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Blog.Application.Services
{
    public sealed class PostsService : IApplicationService
    {
        private readonly IDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<PostsService> _logger;
        private const int RelatedPostsCount = 3;
        
        public PostsService(IDbContext dbContext, IMapper mapper, IDateTimeProvider dateTimeProvider, ILogger<PostsService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
        }

        public async Task<IEnumerable<PostDto>> GetRelatedPosts(Guid postIdToExclude, IEnumerable<Guid> tagsIds, CancellationToken cancellationToken)
        {
            return await _dbContext.Posts
                .AsNoTracking()
                .Where(x => x.IsVisible && x.PublishDate <= _dateTimeProvider.UtcNow && x.Id != postIdToExclude &&
                            x.TagAssignments.Any(f => tagsIds.Contains(f.TagId)))
                .OrderByDescending(x => x.TagAssignments.Count(f => tagsIds.Contains(f.TagId)))
                .ThenByDescending(e => e.CreatedOn)
                .Take(RelatedPostsCount)
                .ProjectTo<PostDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
        
        public async Task<bool> IncrementPostFileDownloadsNumber(Guid postFileId, CancellationToken cancellationToken)
        {
            await using IDbContextTransaction transaction = await _dbContext.BeginTransactionAsync(cancellationToken);
            try
            {
                var affectedRows = await _dbContext.PostFiles.Where(x => x.Id == postFileId)
                    .ExecuteUpdateAsync(x => x.SetProperty(z => z.Counter, z => z.Counter + 1), cancellationToken);
                    
                await transaction.CommitAsync(cancellationToken);
                return affectedRows > 0;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Произошла ошибка при попытке обновить количество скачиваний файла");
                await transaction.RollbackAsync(cancellationToken);
                return false;
            }
        }
    }
}