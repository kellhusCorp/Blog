using Blog.Application.Enums;
using Blog.Application.Providers;
using Blog.Application.Settings;

namespace Blog.Application.Factories
{
    public class FileProviderFactory : IFileProviderFactory
    {
        private readonly string _pathToContentRootDirectory;
        private readonly StorageServicesSettings _storageServicesSettings;

        public FileProviderFactory(string pathToContentRootDirectory, StorageServicesSettings storageServicesSettings)
        {
            _pathToContentRootDirectory = pathToContentRootDirectory;
            _storageServicesSettings = storageServicesSettings;
        }
        
        public IFileProvider GetFileProvider(FileProviderType type)
        {
            switch (type)
            {
                case FileProviderType.File:
                    return new FileProvider(_pathToContentRootDirectory, _storageServicesSettings.InvariantFilesRootDirectory);
                case FileProviderType.Image:
                    return new FileProvider(_pathToContentRootDirectory, _storageServicesSettings.InvariantFilesRootDirectory);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}