using Blog.BLL.Commands;
using Blog.BLL.Handlers;
using Blog.BLL.Providers;
using Blog.Domain;
using Blog.Infrastructure.Contexts;
using Blog.PublicApi.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlogOnCore.Infrastructure.Paging;

namespace Blog.PublicApi.Controllers;

public class PostsController : BaseController
{
    private const int EntriesPerPage = 10;
    
    private readonly BlogDbContext context;
    private readonly ICommandHandler<AddCommentCommand> addCommentHandler;
    private readonly ICommandHandler<IncrementBlogFileCounterCommand> incrementBlogFileCounterCommandHandler;
    private readonly IBlogFileProvider blogFileProvider;
    private readonly IMediator _mediator;

    public PostsController(
        BlogDbContext context,
        ICommandHandler<AddCommentCommand> addCommentHandler, 
        ICommandHandler<IncrementBlogFileCounterCommand> incrementBlogFileCounterCommandHandler,
        IBlogFileProvider blogFileProvider,
        IMediator mediator) 
        : base(context)
    {
        this.context = context;
        this.addCommentHandler = addCommentHandler;
        this.incrementBlogFileCounterCommandHandler = incrementBlogFileCounterCommandHandler;
        this.blogFileProvider = blogFileProvider;
        _mediator = mediator;
    }

    [Authorize(Roles = "Admin")]
    [Route("[controller]/[action]")]
    public async Task<IActionResult> DeleteComment(Guid commentId, string? back)
    {
        await _mediator.Send(new DeleteCommentCommand(commentId));

        if (back != null && !string.IsNullOrEmpty(back) && Url.IsLocalUrl(back))
        {
            return Redirect(back);
        }
        
        return RedirectToAction(nameof(Index));
    }
    
    [Route("")]
    [Route("[controller]")]
    [Route("[controller]/Index")]
    [Route("[controller]/Tag/{tag}")]
    public async Task<ActionResult> Index(Paging<Post> paging, string? tag, string? search)
    {
        paging.SetSortExpression(p => p.PublishDate);
        paging.SortDirection = SortDirection.Descending;
        paging.Top = EntriesPerPage;

        var query = context.Posts
                        .Include(b => b.Author)
                        .Include(b => b.TagAssignments!)
                        .ThenInclude(b => b.Tag)
                        .AsNoTracking()
                        .Where(e => (e.IsVisible && e.PublishDate <= DateTimeOffset.Now)
                            || (this.User.Identity != null && this.User.Identity.IsAuthenticated));

        if (!string.IsNullOrEmpty(tag))
        {
            query = query.Where(e => e.TagAssignments!.Any(t => t.Tag!.Name == tag));
        }

        if (!string.IsNullOrEmpty(search))
        {
            foreach (var item in search.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Where(e => e.Header.Contains(item));
            }
        }

        var entries = await query.GetPagedResultAsync(paging);
        var tags = await context.Tags
            .AsNoTracking()
            .OrderBy(t => t.Name)
            .ToListAsync();
        var popularBlogEntries = await context.Posts
            .AsNoTracking()
            .Where(e => (e.IsVisible && e.PublishDate <= DateTimeOffset.Now)
                || (this.User.Identity != null && this.User.Identity.IsAuthenticated))
            .OrderByDescending(b => b.VisitsNumber)
            .Take(5)
            .ToListAsync();

        var model = new PostsIndexViewModel(entries, tags, popularBlogEntries)
        {
            Tag = tag,
            Search = search
        };

        if (model.Entries.TotalNumberOfItems > 0)
        {
            var ids = model.Entries.Select(e => e.Id).ToList();

            var blogEntryComments = await context.Comments
                .AsNoTracking()
                .Where(b => ids.Contains(b.BlogId!.Value))
                .ToListAsync();

            foreach (var entry in model.Entries)
            {
                entry.Comments = blogEntryComments.Where(b => b.BlogId == entry.Id).ToList();
            }
        }

        return View(model);
    }
    
    [Route("[controller]/{year:int}/{month:int}/{day:int}/{id}")]
    public async Task<IActionResult> Entry(string id)
    {
        var post = await GetByPermanentLink(id);

        if (post == null)
        {
            return NotFound();
        }

        if (User.Identity == null || !User.Identity.IsAuthenticated)
        {
            await _mediator.Send(new IncrementVisitsNumberCommand(post.Id));
            post.VisitsNumber++;
        }

        return View(new BlogViewModel
        {
            BlogEntry = post,
            RelatedBlogEntries = await GetRelatedPosts(post)
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("[controller]/{year:int}/{month:int}/{day:int}/{id}")]
    public async Task<IActionResult> Entry(string id, BlogViewModel model)
    {
        var entry = await GetByPermanentLink(id);

        if (entry == null)
        {
            return NotFound();
        }

        ModelState.Remove(nameof(BlogViewModel.BlogEntry));
        ModelState.Remove(nameof(BlogViewModel.RelatedBlogEntries));

        if (!ModelState.IsValid)
        {
            model.BlogEntry = entry;
            model.RelatedBlogEntries = await GetRelatedPosts(entry);
        }
        
        var comment = new Comment(model.Comment.Name, model.Comment.Comment)
        {
            Email = model.Comment.Email,
            Homepage = model.Comment.Homepage,
            AdminPost = User.Identity is {IsAuthenticated: true} && User.IsInRole("Admin"),
            BlogId = entry.Id
        };

        await addCommentHandler.ExecuteAsync(new AddCommentCommand(comment)
        {
            Referer = Request.GetTypedHeaders().Referer?.ToString()
        });

        return RedirectToAction(nameof(Entry), new {Id = id});
    }

    [HttpGet]
    public async Task<IActionResult> Download(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        
        var blogFile = await context.PostFiles
            .AsNoTracking()
            .SingleOrDefaultAsync(b => b.Id == id);

        if (blogFile == null)
        {
            return NotFound();
        }
        
        if (User.Identity is not { IsAuthenticated: true })
        {
            await incrementBlogFileCounterCommandHandler.ExecuteAsync(new IncrementBlogFileCounterCommand(blogFile.Id));
        }
        
        var data = await blogFileProvider.GetFileAsync(blogFile.Path);

        var file = new FileContentResult(data, "application/octet-stream")
        {
            FileDownloadName = blogFile.Name
        };
        
        return file;
    }
    
    protected sealed override async Task<Post?> GetByPermanentLink(string header)
    {
        var entry =  await base.GetByPermanentLink(header);
        if (entry != null)
        {
            entry.Comments = await context.Comments
                .AsNoTracking()
                .Where(b => b.BlogId == entry.Id)
                .OrderByDescending(b => b.CreatedOn)
                .ToListAsync();
        }

        return entry;
    }
    
    private async Task<List<Post>> GetRelatedPosts(Post post)
    {
        var tagIds = post.TagAssignments!.Select(t => t.TagId).ToList();

        var query = await context.Posts
            .AsNoTracking()
            .Where(e => e.IsVisible && e.PublishDate <= DateTimeOffset.UtcNow && e.Id != post.Id)
            .Where(e => e.TagAssignments!.Any(t => tagIds.Contains(t.TagId)))
            .OrderByDescending(e => e.TagAssignments!.Count(t => tagIds.Contains(t.TagId)))
            .ThenByDescending(e => e.CreatedOn)
            .Take(3)
            .ToListAsync();

        return query;
    }
}