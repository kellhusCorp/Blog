using Blog.Domain;

namespace Blog.BLL.Repositories
{
    public interface IPagesRepository
    {
        Task<Page?> GetByName(string pageName);
        
        IQueryable<Page> GetAll();
    }
}