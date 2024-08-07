using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBlogOnCore.BLL.Commands;
using MyBlogOnCore.DataSource.Contexts;

namespace MyBlogOnCore.BLL.Handlers
{
    [UsedImplicitly]
    public class IncrementVisitsNumberHandler : IRequestHandler<IncrementVisitsNumberCommand>
    {
        private readonly BlogDbContext _context;
        
        public IncrementVisitsNumberHandler(BlogDbContext context)
        {
            _context = context;
        }
        
        public async Task Handle(IncrementVisitsNumberCommand request, CancellationToken cancellationToken = default)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync(
                $"UPDATE \"Blogs\" SET \"VisitsNumber\" = \"VisitsNumber\" + 1 WHERE \"Id\" = {request.BlogId}", cancellationToken: cancellationToken);
        }
    }
}