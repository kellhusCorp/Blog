using Blog.BLL.Providers;
using Blog.Domain;
using Blog.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Blog.BLL.Services;

public class ImageStorageService : IImageStorageService
{
    private readonly BlogDbContext context;
    private readonly IImageFileProvider imageFileProvider;

    public ImageStorageService(BlogDbContext context, IImageFileProvider imageFileProvider)
    {
        this.context = context;
        this.imageFileProvider = imageFileProvider;
    }

    public async Task<string> AddOrUpdate(string filename, byte[] data)
    {
        var fileName = filename.Replace('/', '\\');
        fileName = fileName.Substring(fileName.IndexOf('\\') + 1);

        var extension = fileName.Substring(fileName.LastIndexOf('.') + 1);

        var image = new Image(fileName);

        context.Images.Add(image);

        await imageFileProvider.AddFileAsync($"{image.Id}.{extension}", data);

        await context.SaveChangesAsync();

        return $"{image.Id}.{extension}";
    }

    public async Task Delete(Guid id)
    {
        var entity = await context.Images.SingleOrDefaultAsync(e => e.Id == id);

        if (entity != null)
        {
            context.Images.Remove(entity);

            await context.SaveChangesAsync();

            await imageFileProvider.DeleteFileAsync(entity.Path);
        }
    }
}