using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blog.Application.Contexts;
using Blog.Application.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Services
{
    public sealed class PostsService : IApplicationService
    {
        private readonly IDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _dateTimeProvider;
        private const int RelatedPostsCount = 3;
        public PostsService(IDbContext dbContext, IMapper mapper, IDateTimeProvider dateTimeProvider)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _dateTimeProvider = dateTimeProvider;
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
    }
}