using System.Collections.ObjectModel;
using Blog.Application.Services;
using Blog.Application.Settings;
using Blog.BLL.Commands;
using Blog.BLL.Handlers;
using Blog.Domain.Entities;
using Blog.Infrastructure.Contexts;
using Blog.Localization;
using Blog.PublicApi.Infrastructure.Paging;
using Blog.PublicApi.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyBlogOnCore.Models;

namespace Blog.PublicApi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministrationController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ICommandHandler<AddOrUpdateBlogFileCommand> addOrUpdateBlogFileCommandHandler;
        private readonly BlogDbContext context;
        private readonly string imagesRootDirectory;
        private readonly IImageStorageService imageStorageService;
        private readonly UserManager<User> userManager;

        public AdministrationController(
            BlogDbContext context,
            UserManager<User> userManager,
            IImageStorageService imageStorageService,
            IOptionsMonitor<StorageServicesSettings> servicesSettings,
            ICommandHandler<AddOrUpdateBlogFileCommand> addOrUpdateBlogFileCommandHandler,
            IMediator mediator) : base(context)
        {
            this.context = context;
            this.userManager = userManager;
            this.imageStorageService = imageStorageService;
            this.addOrUpdateBlogFileCommandHandler = addOrUpdateBlogFileCommandHandler;
            _mediator = mediator;
            imagesRootDirectory = servicesSettings.CurrentValue.ImagesRootDirectory;
        }

        public async Task<IActionResult> Index(IndexViewModel model, Paging<Post> paging, bool? download)
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

            IQueryable<Post> query = context.Posts
                .AsNoTracking();

            if (model.SearchTerm != null)
            {
                query = query.Where(u => u.Header.Contains(model.SearchTerm));
            }

            model.Posts = await query
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
            Post? post;

            if (id == null)
            {
                post = new Post(string.Empty, string.Empty, string.Empty)
                {
                    AuthorId = userManager.GetUserId(User),
                    PublishDate = DateTimeOffset.UtcNow,
                    TagAssignments = new Collection<TagAssignment>()
                };
            }
            else
            {
                post = await GetByPermanentLink(id);

                if (post == null)
                {
                    return NotFound();
                }
            }

            EditPostViewModel model = new(post);

            model.SelectedTagNames = model.Post.TagAssignments!
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
        public async Task<ActionResult> EditBlog(string? id, EditPostViewModel model)
        {
            if (id == null && model.Post.PermanentLink == null)
            {
                ModelState.Remove($"{nameof(EditPostViewModel.Post)}.{nameof(EditPostViewModel.Post.PermanentLink)}");
            }

            if (!ModelState.IsValid)
            {
                await SetTagsAndAuthors(model);
                return View(model);
            }
            
            await _mediator.Send(new AddOrUpdateBlogCommand(model.Post, model.SelectedTagNames));

            SetSuccessMessage(Resources.SavedSuccessfully);

            return RedirectToAction(nameof(EditBlog), new {year = model.Post.PublishDate.Year, month = model.Post.PublishDate.Month, day = model.Post.PublishDate.Day, id = model.Post.PermanentLink});
        }

        public async Task<IActionResult> Images(ImagesViewModel model, Paging<Image> paging)
        {
            if (paging.SortColumn == null)
            {
                paging.SetSortExpression(p => p.CreatedOn);
                paging.SortDirection = SortDirection.Descending;
            }

            IQueryable<Image> query = context.Images
                .AsNoTracking();

            if (model.SearchTerm != null)
            {
                query = query.Where(u => u.Name.Contains(model.SearchTerm));
            }

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
                foreach (string part in model.SearchTerm.Replace(",", string.Empty).Split(' ', StringSplitOptions.RemoveEmptyEntries))
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
            if (!ModelState.IsValid)
            {
                return RedirectToAction();
            }

            using (MemoryStream ms = new())
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
            if (upload.Length <= 0)
            {
                return null;
            }

            using (MemoryStream ms = new())
            {
                await upload.OpenReadStream().CopyToAsync(ms);

                string fileName = await imageStorageService.AddOrUpdate(
                    upload.FileName,
                    ms.ToArray());

                const string successMessage = "image is uploaded";

                return Json(new
                {
                    uploaded = 1,
                    fileName,
                    url = $"{GetRootDirectoryForImage()}/{fileName}",
                    error = new { message = successMessage }
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

            using (MemoryStream ms = new())
            {
                await model.File?.CopyToAsync(ms)!;
                byte[] file = ms.ToArray();

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
            if (!id.HasValue)
            {
                return NotFound();
            }

            await imageStorageService.Delete(id.Value);

            SetSuccessMessage(Resources.DeletedSuccessfully);

            return RedirectToAction(nameof(Images));
        }

        private async Task SetTagsAndAuthors(EditPostViewModel model)
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

            if (model.Post.Files == null)
            {
                model.Post.Files = await context.PostFiles
                    .AsNoTracking()
                    .Where(b => b.BlogId == model.Post.Id)
                    .ToListAsync();
            }
        }
    }
}