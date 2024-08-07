using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Blog.BLL.Commands;
using Blog.BLL.Exceptions;
using Blog.Domain;
using Blog.Infrastructure.Contexts;
using Blog.Localization;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.BLL.Handlers
{
    public class AddOrUpdateBlogHandler : IRequestHandler<AddOrUpdateBlogCommand>
    {
        private readonly BlogDbContext _context;
        
        public AddOrUpdateBlogHandler(BlogDbContext context)
        {
            _context = context;
        }

        public async Task Handle(AddOrUpdateBlogCommand request, CancellationToken cancellationToken = default)
        {
            var blogEntryWithSamePermalink = _context.Posts
                .Any(b => b.Id != request.Post.Id && b.PermanentLink == request.Post.PermanentLink);

            if (blogEntryWithSamePermalink)
            {
                throw new BusinessException(string.Format(Resources.PermanentLinkInUse, request.Post.PermanentLink));
            }

            var blog = _context.Posts
                .Include(b => b.TagAssignments!)
                .ThenInclude(b => b.Tag)
                .SingleOrDefault(b => b.Id == request.Post.Id);

            if (blog == null)
            {
                blog = request.Post;
                blog.PermanentLink = Regex.Replace(
                    blog.Header.ToLowerInvariant().Replace(" - ", "-").Replace(" ", "-"),
                    "[^\\w^-]",
                    string.Empty);

                blog.UpdateDate = blog.CreatedOn;

                _context.Posts.Add(blog);
            }
            else
            {
                blog.UpdateDate = DateTimeOffset.UtcNow;
                blog.Header = request.Post.Header;
                blog.PermanentLink = request.Post.PermanentLink;
                blog.ShortContent = request.Post.ShortContent;
                blog.Body = request.Post.Body;
                blog.AuthorId = request.Post.AuthorId;
                blog.PublishDate = request.Post.PublishDate;
                blog.IsVisible = request.Post.IsVisible;
            }

            await AddTagsAsync(blog, request.Tags);

            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task AddTagsAsync(Domain.Post post, IEnumerable<string> tags)
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