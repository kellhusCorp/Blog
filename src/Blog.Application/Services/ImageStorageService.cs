using Blog.Application.Contexts;
using Blog.Application.Providers;
using Blog.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Services;

public class ImageStorageService : IImageStorageService
{
    private readonly IDbContext _context;
    private readonly IImageFileProvider imageFileProvider;

    public ImageStorageService(IDbContext context, IImageFileProvider imageFileProvider)
    {
        _context = context;
        this.imageFileProvider = imageFileProvider;
    }

    public async Task<string> AddOrUpdate(string filename, byte[] data)
    {
        var fileName = filename.Replace('/', '\\');
        fileName = fileName.Substring(fileName.IndexOf('\\') + 1);

        var extension = fileName.Substring(fileName.LastIndexOf('.') + 1);

        var image = new Image(fileName);

        _context.Images.Add(image);

        await imageFileProvider.AddFileAsync($"{image.Id}.{extension}", data);

        await _context.SaveChangesAsync();

        return $"{image.Id}.{extension}";
    }

    public async Task Delete(Guid id)
    {
        var entity = await _context.Images.SingleOrDefaultAsync(e => e.Id == id);

        if (entity != null)
        {
            _context.Images.Remove(entity);

            await _context.SaveChangesAsync();

            await imageFileProvider.DeleteFileAsync(entity.Path);
        }
    }
}