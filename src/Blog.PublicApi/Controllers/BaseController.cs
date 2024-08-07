using Blog.Domain;
using Blog.Infrastructure.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.PublicApi.Controllers;

public class BaseController : Controller
{
    private readonly BlogDbContext context;

    public const string SuccessMessage = "SuccessMessage";

    public const string WarningMessage = "WarningMessage";

    public const string ErrorMessage = "ErrorMessage";

    public BaseController(BlogDbContext context)
    {
        this.context = context;
    }

    public void SetSuccessMessage( string message)
    {
        TempData[SuccessMessage] = message;
    }

    public void SetWarningMessage( string message)
    {
        TempData[WarningMessage] = message;
    }

    public void SetErrorMessage( string message)
    {
        TempData[ErrorMessage] = message;
    }

    protected virtual async Task<Post?> GetByPermanentLink(string header)
    {
        var entry = await context.Posts
            .Include(x => x.Author)
            .Include(b => b.TagAssignments!)
            .ThenInclude(b => b.Tag)
            .Include(b => b.Files)
            .AsNoTracking()
            .SingleOrDefaultAsync(e => e.PermanentLink.Equals(header));

        return entry;
    }
}