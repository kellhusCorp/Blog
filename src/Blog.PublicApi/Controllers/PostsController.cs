using AutoMapper;
using Blog.Application.UseCases.AddPostComment;
using Blog.Application.UseCases.DeletePostComment;
using Blog.Application.UseCases.DownloadPostFile;
using Blog.Application.UseCases.GetPostByLink;
using Blog.BLL.Commands;
using Blog.BLL.Handlers;
using Blog.Domain;
using Blog.Domain.Entities;
using Blog.Infrastructure.Contexts;
using Blog.PublicApi.Infrastructure.Paging;
using Blog.PublicApi.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.PublicApi.Controllers;

public class PostsController : BaseController
{
    private const int EntriesPerPage = 10;

    private readonly BlogDbContext _context;
    private readonly ICommandHandler<AddCommentCommand> _addCommentHandler;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public PostsController(
        BlogDbContext context,
        ICommandHandler<AddCommentCommand> addCommentHandler,
        IMediator mediator,
        IMapper mapper)
        : base(context)
    {
        _context = context;
        _addCommentHandler = addCommentHandler;
        _mediator = mediator;
        _mapper = mapper;
    }

    [Authorize(Roles = "Admin")]
    [Route("[controller]/[action]")]
    public async Task<IActionResult> DeleteComment(Guid commentId, string? back)
    {
        var result = await _mediator.Send(new DeleteCommentCommand(commentId));

        if (!result.IsSuccessful)
        {
            SetErrorMessage(result.ErrorMessage);
        }
        else
        {
            SetSuccessMessage("Комментарий успешно удалён");
        }

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

        var query = _context.Posts
            .Include(b => b.Author)
            .Include(b => b.TagAssignments!)
            .ThenInclude(b => b.Tag)
            .AsNoTracking()
            .Where(e => (e.IsVisible && e.PublishDate <= DateTimeOffset.Now)
                        || (User.Identity != null && User.Identity.IsAuthenticated));

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
        var tags = await _context.Tags
            .AsNoTracking()
            .OrderBy(t => t.Name)
            .ToListAsync();
        var popularBlogEntries = await _context.Posts
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

            var blogEntryComments = await _context.Comments
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
    public async Task<IActionResult> Entry(string id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPostWithRelatedByLinkQuery(id), cancellationToken);
        return View(_mapper.Map<PostViewModel>(result.Value));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("[controller]/{year:int}/{month:int}/{day:int}/{id}")]
    public async Task<IActionResult> Entry(string id, BlogViewModel model)
    {
        var result = await _mediator.Send()
        
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
            AdminPost = User.Identity is { IsAuthenticated: true } && User.IsInRole("Admin"),
            BlogId = entry.Id
        };

        await _addCommentHandler.ExecuteAsync(new AddCommentCommand(comment) { Referer = Request.GetTypedHeaders().Referer?.ToString() });

        return RedirectToAction(nameof(Entry), new { Id = id });
    }

    [HttpGet]
    public async Task<IActionResult> Download(Guid id)
    {
        var result = await _mediator.Send(new DownloadPostFileQuery(id));
        
        if (!result.IsSuccessful)
        {
            return BadRequest(result.ErrorMessage);
        }

        return new FileContentResult(result.Value.File, result.Value.ContentType) { FileDownloadName = result.Value.FileName };
    }

    protected sealed override async Task<Post?> GetByPermanentLink(string header)
    {
        var entry = await base.GetByPermanentLink(header);
        if (entry != null)
        {
            entry.Comments = await _context.Comments
                .AsNoTracking()
                .Where(b => b.BlogId == entry.Id)
                .OrderByDescending(b => b.CreatedOn)
                .ToListAsync();
        }

        return entry;
    }

    private async Task<List<Post>> GetRelatedPosts(Post post)
    {
        var tagIds = post.TagAssignments.Select(t => t.TagId).ToList();

        var query = await _context.Posts
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