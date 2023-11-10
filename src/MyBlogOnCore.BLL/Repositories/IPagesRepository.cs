using MyBlogOnCore.Domain;

namespace MyBlogOnCore.BLL.Repositories
{
    public interface IPagesRepository
    {
        Task<Page?> GetByName(string pageName);
        
        IQueryable<Page> GetAll();
    }
}