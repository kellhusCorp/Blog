using AutoMapper;
using Blog.Application.Contexts;
using Blog.Domain.Entities;
using Ganss.Xss;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.UseCases.AddPostComment
{
    public class AddPostCommentHandler : IRequestHandler<AddPostCommentCommand, Result<Unit>>
    {
        private readonly IDbContext _dbContext;
        private readonly IMapper _mapper;

        public AddPostCommentHandler(IDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        
        public async Task<Result<Unit>> Handle(AddPostCommentCommand request, CancellationToken cancellationToken)
        {
            if (!await _dbContext.Posts.AnyAsync(x => x.PermanentLink.Equals(request.Link), cancellationToken))
            {
                return Result<Unit>.Failure("Пост не найден");
            }
            
            var sanitizer = new HtmlSanitizer();

            request.Body = sanitizer.Sanitize(request.Body);

            var comment = _mapper.Map<Comment>(request);
            
            await _dbContext.Comments.AddAsync(comment, cancellationToken);
            
            await _dbContext.SaveChangesAsync(cancellationToken);
            
            return Result<Unit>.Success(Unit.Value);
        }
    }
}