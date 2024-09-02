using Blog.Application.Contexts;
using Blog.Application.Enums;
using Blog.Application.Factories;
using Blog.Application.Providers;
using Blog.Domain;
using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Services;

public class ImageStorageService : IImageStorageService
{
    private readonly IDbContext _context;
    private readonly IFileProviderFactory _fileProviderFactory;

    public ImageStorageService(IDbContext context, IFileProviderFactory fileProviderFactory)
    {
        _context = context;
        _fileProviderFactory = fileProviderFactory;
    }

    public async Task<string> AddOrUpdate(string filename, byte[] data)
    {
        var fileName = filename.Replace('/', '\\');
        fileName = fileName.Substring(fileName.IndexOf('\\') + 1);

        var extension = fileName.Substring(fileName.LastIndexOf('.') + 1);

        var image = new Image(fileName);

        _context.Images.Add(image);

        var provider = _fileProviderFactory.GetFileProvider(FileProviderType.Image);
        
        await provider.AddFileAsync($"{image.Id}.{extension}", data);

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

            var provider = _fileProviderFactory.GetFileProvider(FileProviderType.Image);
            await provider.DeleteFileAsync(entity.Path);
        }
    }
}