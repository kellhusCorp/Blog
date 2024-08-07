using Microsoft.EntityFrameworkCore;
using MyBlogOnCore.DataSource.Contexts;
using MyBlogOnCore.Domain;

namespace MyBlogOnCore.BLL.Repositories
{
    public class PagesRepository : IPagesRepository
    {
        private readonly BlogDbContext _context;

        public PagesRepository(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<Page?> GetByName(string pageName)
        {
            return await _context.Pages
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Name.ToLower() == pageName.ToLower());
        }

        public IQueryable<Page> GetAll()
        {
            return _context.Pages;
        }
    }
}