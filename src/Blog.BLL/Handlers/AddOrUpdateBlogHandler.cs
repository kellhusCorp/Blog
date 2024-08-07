using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBlogOnCore.BLL.Commands;
using MyBlogOnCore.BLL.Exceptions;
using MyBlogOnCore.DataSource.Contexts;
using MyBlogOnCore.Domain;
using MyBlogOnCore.Localization;

namespace MyBlogOnCore.BLL.Handlers
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
            var blogEntryWithSamePermalink = _context.Blogs
                .Any(b => b.Id != request.Blog.Id && b.PermanentLink == request.Blog.PermanentLink);

            if (blogEntryWithSamePermalink)
            {
                throw new BusinessException(string.Format(Resources.PermanentLinkInUse, request.Blog.PermanentLink));
            }

            var blog = _context.Blogs
                .Include(b => b.TagAssignments!)
                .ThenInclude(b => b.Tag)
                .SingleOrDefault(b => b.Id == request.Blog.Id);

            if (blog == null)
            {
                blog = request.Blog;
                blog.PermanentLink = Regex.Replace(
                    blog.Header.ToLowerInvariant().Replace(" - ", "-").Replace(" ", "-"),
                    "[^\\w^-]",
                    string.Empty);

                blog.UpdateDate = blog.CreatedOn;

                _context.Blogs.Add(blog);
            }
            else
            {
                blog.UpdateDate = DateTimeOffset.UtcNow;
                blog.Header = request.Blog.Header;
                blog.PermanentLink = request.Blog.PermanentLink;
                blog.ShortContent = request.Blog.ShortContent;
                blog.Body = request.Blog.Body;
                blog.AuthorId = request.Blog.AuthorId;
                blog.PublishDate = request.Blog.PublishDate;
                blog.IsVisible = request.Blog.IsVisible;
            }

            await AddTagsAsync(blog, request.Tags);

            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task AddTagsAsync(Blog blog, IEnumerable<string> tags)
        {
            var existingTags = await _context.Tags.ToListAsync();

            if (blog.TagAssignments == null)
            {
                blog.TagAssignments = new Collection<TagAssignment>();
            }

            foreach (var tag in blog.TagAssignments.Where(t => !tags.Contains(t.Tag!.Name)).ToArray())
            {
                blog.TagAssignments.Remove(tag);
            }

            foreach (var tag in tags.Where(t => !blog.TagAssignments.Select(et => et.Tag!.Name).Contains(t)).ToArray())
            {
                var existingTag = existingTags.SingleOrDefault(t => t.Name.Equals(tag, StringComparison.OrdinalIgnoreCase));

                if (existingTag == null)
                {
                    existingTag = new Tag(tag);
                    existingTags.Add(existingTag);
                    _context.Tags.Add(existingTag);
                }

                blog.TagAssignments.Add(new TagAssignment
                {
                    BlogId = blog.Id,
                    TagId = existingTag.Id
                });
            }
        }
    }
}