using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Blog.Application.Contexts;
using Blog.Domain.Entities;
using Blog.Localization;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.UseCases.UpdatePost
{
    public class UpdatePostHandler : IRequestHandler<UpdatePostCommand, Result<Unit>>
    {
        private readonly IDbContext _context;
        
        public UpdatePostHandler(IDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Unit>> Handle(UpdatePostCommand request, CancellationToken cancellationToken = default)
        {
            var blogEntryWithSamePermalink = _context.Posts
                .Any(b => b.Id != request.Post.Id && b.PermanentLink == request.Post.PermanentLink);

            if (blogEntryWithSamePermalink)
            {
                return Result<Unit>.Failure(string.Format(Resources.PermanentLinkInUse, request.Post.PermanentLink));
            }

            var post = await _context.Posts
                .Include(b => b.TagAssignments)
                .ThenInclude(b => b.Tag)
                .FirstOrDefaultAsync(b => b.Id == request.Post.Id, cancellationToken);

            if (post is null)
            {
                post = request.Post;
                post.PermanentLink = Regex.Replace(
                    post.Header.ToLowerInvariant().Replace(" - ", "-").Replace(" ", "-"),
                    "[^\\w^-]",
                    string.Empty);

                post.UpdateDate = post.CreatedOn;

                _context.Posts.Add(post);
            }
            else
            {
                post.UpdateDate = DateTimeOffset.UtcNow;
                post.Header = request.Post.Header;
                post.PermanentLink = request.Post.PermanentLink;
                post.ShortContent = request.Post.ShortContent;
                post.Body = request.Post.Body;
                post.AuthorId = request.Post.AuthorId;
                post.PublishDate = request.Post.PublishDate;
                post.IsVisible = request.Post.IsVisible;
            }

            await AddTagsAsync(post, request.Tags);

            await _context.SaveChangesAsync(cancellationToken);
            
            return Result<Unit>.Success(Unit.Value);
        }

        private async Task AddTagsAsync(Post post, IEnumerable<string> tags)
        {
            var existingTags = await _context.Tags.ToListAsync();

            if (post.TagAssignments == null)
            {
                post.TagAssignments = new Collection<TagAssignment>();
            }

            foreach (var tag in post.TagAssignments.Where(t => !tags.Contains(t.Tag!.Name)).ToArray())
            {
                post.TagAssignments.Remove(tag);
            }

            foreach (var tag in tags.Where(t => !post.TagAssignments.Select(et => et.Tag!.Name).Contains(t)).ToArray())
            {
                var existingTag = existingTags.SingleOrDefault(t => t.Name.Equals(tag, StringComparison.OrdinalIgnoreCase));

                if (existingTag == null)
                {
                    existingTag = new Tag(tag);
                    existingTags.Add(existingTag);
                    _context.Tags.Add(existingTag);
                }

                post.TagAssignments.Add(new TagAssignment
                {
                    BlogId = post.Id,
                    TagId = existingTag.Id
                });
            }
        }
    }
}