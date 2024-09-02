using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Blog.Application.Contexts;
using Blog.Application.Dtos;
using Blog.Application.Services;
using Blog.Domain;
using Blog.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.UseCases.GetPostByLink
{
    public class GetPostWithRelatedByLinkQuery : IRequest<Result<PostWithRelatedPostsDto>>
    {
        private string PermanentLink { get; set; } = null!;

        public GetPostWithRelatedByLinkQuery(string permanentLink)
        {
            PermanentLink = permanentLink;
        }


        private class GetPostByLinkHandler : IRequestHandler<GetPostWithRelatedByLinkQuery, Result<PostWithRelatedPostsDto>>
        {
            private readonly IDbContext _dbContext;
            private readonly IMapper _mapper;
            private readonly PostsService _postsService;

            public GetPostByLinkHandler(IDbContext dbContext, IMapper mapper, PostsService postsService)
            {
                _dbContext = dbContext;
                _mapper = mapper;
                _postsService = postsService;
            }

            [SuppressMessage("ReSharper.DPA", "DPA0007: Large number of DB records", MessageId = "count: 152")]
            public async Task<Result<PostWithRelatedPostsDto>> Handle(GetPostWithRelatedByLinkQuery request, CancellationToken cancellationToken)
            {
                var post = await _dbContext.Posts
                    .Include(x => x.Author)
                    .Include(x => x.TagAssignments)
                        .ThenInclude(x => x.Tag)
                    .Include(x => x.Comments)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.PermanentLink == request.PermanentLink, cancellationToken);

                if (post is null)
                {
                    return Result<PostWithRelatedPostsDto>.Failure("Пост не найден");
                }
                
                var dto = new PostWithRelatedPostsDto
                {
                    Post = _mapper.Map<PostDto>(post),
                    RelatedPosts = await _postsService.GetRelatedPosts(post.Id, post.TagAssignments.Select(x => x.TagId), cancellationToken)
                };

                return Result<PostWithRelatedPostsDto>.Success(dto);
            }
        }
    }
}