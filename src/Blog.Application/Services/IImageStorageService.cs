namespace Blog.Application.Services;

public interface IImageStorageService
{
    Task<string> AddOrUpdate(string filename, byte[] data);

    Task Delete(Guid id);
}