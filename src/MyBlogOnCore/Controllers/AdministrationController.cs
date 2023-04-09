using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyBlogOnCore.BLL;
using MyBlogOnCore.BLL.Commands;
using MyBlogOnCore.BLL.Exceptions;
using MyBlogOnCore.BLL.Handlers;
using MyBlogOnCore.BLL.Services;
using MyBlogOnCore.DataSource.Contexts;
using MyBlogOnCore.Domain;
using MyBlogOnCore.Infrastructure.Paging;
using MyBlogOnCore.Localization;
using MyBlogOnCore.Models;

namespace MyBlogOnCore.Controllers;

[Authorize(Roles = "Admin")]
public class AdministrationController : BaseController
{
    private readonly IBlogService<Blog> blogService;
    private readonly BlogDbContext context;
    private readonly string imagesRootDirectory;
    private readonly IImageStorageService imageStorageService;
    private readonly ICommandHandler<AddOrUpdateBlogFileCommand> addOrUpdateBlogFileCommandHandler;
    private readonly UserManager<User> userManager;

    public AdministrationController(
        BlogDbContext context,
        UserManager<User> userManager,
        IBlogService<Blog> blogService,
        IImageStorageService imageStorageService,
        IOptionsMonitor<StorageServicesSettings> servicesSettings,
        ICommandHandler<AddOrUpdateBlogFileCommand> addOrUpdateBlogFileCommandHandler) : base(context)
    {
        this.context = context;
        this.userManager = userManager;
        this.blogService = blogService;
        this.imageStorageService = imageStorageService;
        this.addOrUpdateBlogFileCommandHandler = addOrUpdateBlogFileCommandHandler;
        imagesRootDirectory = servicesSettings.CurrentValue.ImagesRootDirectory;
    }

    public async Task<IActionResult> Index(IndexViewModel model, Paging<Blog> paging, bool? download)
    {
        if (paging.SortColumn == null)
        {
            paging.SetSortExpression(p => p.PublishDate);
            paging.SortDirection = SortDirection.Descending;
        }

        if (download.GetValueOrDefault())
        {
            paging.Top = int.MaxValue;
            paging.Skip = 0;
        }

        var query = context.Blogs
            .AsNoTracking();

        if (model.SearchTerm != null) query = query.Where(u => u.Header.Contains(model.SearchTerm));

        model.Blogs = await query
            .GetPagedResultAsync(paging);

        // if (download.GetValueOrDefault())
        // {
        //     var document = this.GenerateDocument(model.Blogs);
        //     return this.File(document, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Posts.xlsx");
        // }

        return View(model);
    }

    [Route("blog/{year:int}/{month:int}/{day:int}/{id}/edit")]
    [Route("[controller]/editblog")]
    public async Task<IActionResult> EditBlog(string? id)
    {
        Blog? entry;

        if (id == null)
        {
            entry = new Blog(string.Empty, string.Empty, string.Empty)
            {
                AuthorId = userManager.GetUserId(User),
                PublishDate = DateTimeOffset.UtcNow,
                TagAssignments = new Collection<TagAssignment>()
            };
        }
        else
        {
            entry = await GetByPermanentLink(id);

            if (entry == null) return NotFound();
        }

        var model = new EditBlogViewModel(entry);

        model.SelectedTagNames = model.Blog.TagAssignments!
            .Select(t => t.Tag!.Name)
            .OrderBy(t => t)
            .ToList();

        await SetTagsAndAuthors(model);

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("blog/{year:int}/{month:int}/{day:int}/{id}/edit")]
    [Route("[controller]/editblog")]
    public async Task<ActionResult> EditBlog(string? id, EditBlogViewModel model)
    {
        if (id == null && model.Blog.PermanentLink == null) ModelState.Remove($"{nameof(EditBlogViewModel.Blog)}.{nameof(EditBlogViewModel.Blog.PermanentLink)}");

        if (!ModelState.IsValid)
        {
            await SetTagsAndAuthors(model);
            return View(model);
        }

        try
        {
            await blogService.AddOrUpdate(
                model.Blog,
                model.SelectedTagNames);
        }
        catch (BusinessException ex)
        {
            SetErrorMessage(ex.Message);
            await SetTagsAndAuthors(model);
            return View(model);
        }

        SetSuccessMessage(Resources.SavedSuccessfully);

        return Redirect($"/blog/{model.Blog.Url}/edit");
    }

    public async Task<IActionResult> Images(ImagesViewModel model, Paging<Image> paging)
    {
        if (paging.SortColumn == null)
        {
            paging.SetSortExpression(p => p.CreatedOn);
            paging.SortDirection = SortDirection.Descending;
        }

        var query = context.Images
            .AsNoTracking();

        if (model.SearchTerm != null) query = query.Where(u => u.Name.Contains(model.SearchTerm));

        model.Images = await query
            .GetPagedResultAsync(paging);

        return View(model);
    }

    public async Task<IActionResult> Users(UsersViewModel model, Paging<User> paging)
    {
        if (paging.SortColumn == null)
        {
            paging.SetSortExpression(x => x.LastName);
        }

        IQueryable<User> query = context.Users.AsNoTracking();

        if (model.SearchTerm != null)
        {
            foreach (var part in model.SearchTerm.Replace(",", string.Empty).Split(' ', StringSplitOptions.RemoveEmptyEntries)) 
            {
                if (part.Contains('@'))
                {
                    query = query.Where(x => x.Email.Contains(part));
                }
                else
                {
                    query = query.Where(x => x.LastName.Contains(part) || x.FirstName.Contains(part));
                }
            }
        }

        model.Users = await query.GetPagedResultAsync(paging);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Images(ImagesViewModel model)
    {
        if (!ModelState.IsValid) return RedirectToAction();

        using (var ms = new MemoryStream())
        {
            await model.Image!.OpenReadStream().CopyToAsync(ms);

            await imageStorageService.AddOrUpdate(
                model.Image.FileName,
                ms.ToArray());
        }

        SetSuccessMessage(Resources.SavedSuccessfully);

        return RedirectToAction();
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> UploadImage([FromForm] IFormFile upload, string responseType)
    {
        if (upload.Length <= 0) return null;

        //your custom code logic here

        //1)check if the file is image

        //2)check if the file is too large

        //etc

        using (var ms = new MemoryStream())
        {
            await upload.OpenReadStream().CopyToAsync(ms);

            var fileName = await imageStorageService.AddOrUpdate(
                upload.FileName,
                ms.ToArray());

            const string successMessage = "image is uploaded";

            return Json(new
            {
                uploaded = 1, 
                fileName = fileName,
                url = $"{GetRootDirectoryForImage()}/{fileName}",
                error = new
                {
                    message = successMessage
                }
            });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddBlogFile(AddBlogFileViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return Redirect(Request.Headers["Referer"]);
        }

        using (var ms = new MemoryStream())
        {
            await model.File?.CopyToAsync(ms)!;
            var file = ms.ToArray();
            
            await addOrUpdateBlogFileCommandHandler.ExecuteAsync(new AddOrUpdateBlogFileCommand(
                model.File!.FileName,
                file,
                model.BlogId!.Value));
        }
        
        SetSuccessMessage(Resources.SavedSuccessfully);
        
        return Redirect(Request.Headers["Referer"]);
    }

    private string GetRootDirectoryForImage()
    {
        return imagesRootDirectory
            .Replace("wwwroot/", string.Empty)
            .Replace("\\", "//");
    }

    public async Task<ActionResult> DeleteImage(Guid? id)
    {
        if (!id.HasValue) return NotFound();

        await imageStorageService.Delete(id.Value);

        SetSuccessMessage(Resources.DeletedSuccessfully);

        return RedirectToAction(nameof(Images));
    }

    private async Task SetTagsAndAuthors(EditBlogViewModel model)
    {
        model.AllTags = await context.Tags
            .AsNoTracking()
            .OrderBy(t => t.Name)
            .ToListAsync();

        model.Authors = await context.Users
            .AsNoTracking()
            .OrderBy(t => t.LastName)
            .ThenBy(t => t.FirstName)
            .ToListAsync();

        if (model.Blog.Files == null)
        {
            model.Blog.Files = await context.Files
                .AsNoTracking()
                .Where(b => b.BlogId == model.Blog.Id)
                .ToListAsync();
        }
    }
}