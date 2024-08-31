﻿using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Blog.Application.Contexts;
using Blog.BLL.Exceptions;
using Blog.BLL.Services;
using Blog.Domain;
using Blog.Localization;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Services;

public class BlogService : IBlogService<Post>
{
    private readonly IDbContext _context;

    public BlogService(IDbContext context)
    {
        _context = context;
    }

    public async Task AddOrUpdate(Domain.Post entity, IEnumerable<string> tags)
    {
        var blogEntryWithSamePermalink = _context.Posts
            .Any(b => b.Id != entity.Id && b.PermanentLink == entity.PermanentLink);

        if (blogEntryWithSamePermalink)
        {
            throw new BusinessException(string.Format(Resources.PermanentLinkInUse, entity.PermanentLink));
        }

        var blogEntry = _context.Posts
            .Include(b => b.TagAssignments!)
            .ThenInclude(b => b.Tag)
            .SingleOrDefault(b => b.Id == entity.Id);

        if (blogEntry == null)
        {
            blogEntry = entity;
            blogEntry.PermanentLink = Regex.Replace(
                blogEntry.Header.ToLowerInvariant().Replace(" - ", "-").Replace(" ", "-"),
                "[^\\w^-]",
                string.Empty);

            blogEntry.UpdateDate = blogEntry.CreatedOn;

            _context.Posts.Add(blogEntry);
        }
        else
        {
            blogEntry.UpdateDate = DateTimeOffset.UtcNow;
            blogEntry.Header = entity.Header;
            blogEntry.PermanentLink = entity.PermanentLink;
            blogEntry.ShortContent = entity.ShortContent;
            blogEntry.Body = entity.Body;
            blogEntry.AuthorId = entity.AuthorId;
            blogEntry.PublishDate = entity.PublishDate;
            blogEntry.IsVisible = entity.IsVisible;
        }

        await AddTagsAsync(blogEntry, tags);

        await _context.SaveChangesAsync();
    }

    private async Task AddTagsAsync(Post entry, IEnumerable<string> tags)
    {
        var existingTags = await _context.Tags.ToListAsync();

        if (entry.TagAssignments == null)
        {
            entry.TagAssignments = new Collection<TagAssignment>();
        }

        foreach (var tag in entry.TagAssignments.Where(t => !tags.Contains(t.Tag!.Name)).ToArray())
        {
            entry.TagAssignments.Remove(tag);
        }

        foreach (var tag in tags.Where(t => !entry.TagAssignments.Select(et => et.Tag!.Name).Contains(t)).ToArray())
        {
            var existingTag = existingTags.SingleOrDefault(t => t.Name.Equals(tag, StringComparison.OrdinalIgnoreCase));

            if (existingTag == null)
            {
                existingTag = new Tag(tag);
                existingTags.Add(existingTag);
                _context.Tags.Add(existingTag);
            }

            entry.TagAssignments.Add(new TagAssignment()
            {
                BlogId = entry.Id,
                TagId = existingTag.Id
            });
        }
    }
}