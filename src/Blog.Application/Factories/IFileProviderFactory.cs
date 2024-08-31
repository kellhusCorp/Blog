using Blog.Application.Enums;
using Blog.Application.Providers;

namespace Blog.Application.Factories
{
    public interface IFileProviderFactory
    {
        IFileProvider GetFileProvider(FileProviderType type);
    }
}