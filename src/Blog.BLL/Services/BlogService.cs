﻿using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Blog.BLL.Exceptions;
using Blog.Domain;
using Blog.Infrastructure.Contexts;
using Blog.Localization;
using Microsoft.EntityFrameworkCore;

namespace Blog.BLL.Services;

public class BlogService : IBlogService<Domain.Post>
{
    private readonly BlogDbContext context;

    public BlogService(BlogDbContext context)
    {
        this.context = context;
    }

    public async Task AddOrUpdate(Domain.Post entity, IEnumerable<string> tags)
    {
        var blogEntryWithSamePermalink = context.Posts
            .Any(b => b.Id != entity.Id && b.PermanentLink == entity.PermanentLink);

        if (blogEntryWithSamePermalink)
        {
            throw new BusinessException(string.Format(Resources.PermanentLinkInUse, entity.PermanentLink));
        }

        var blogEntry = context.Posts
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

            context.Posts.Add(blogEntry);
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

        await context.SaveChangesAsync();
    }

    public async Task IncrementVisitsNumber(Guid id)
    {
        await context.Database.ExecuteSqlInterpolatedAsync(
            $"UPDATE \"Blogs\" SET \"VisitsNumber\" = \"VisitsNumber\" + 1 WHERE \"Id\" = {id}");
    }

    private async Task AddTagsAsync(Domain.Post entry, IEnumerable<string> tags)
    {
        var existingTags = await context.Tags.ToListAsync();

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

                context.Tags.Add(existingTag);
            }

            entry.TagAssignments.Add(new TagAssignment()
            {
                BlogId = entry.Id,
                TagId = existingTag.Id
            });
        }
    }
}