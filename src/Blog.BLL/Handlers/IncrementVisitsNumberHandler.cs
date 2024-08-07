using Blog.BLL.Commands;
using Blog.Infrastructure.Contexts;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.BLL.Handlers
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
                $"UPDATE \"Posts\" SET \"VisitsNumber\" = \"VisitsNumber\" + 1 WHERE \"Id\" = {request.BlogId}", cancellationToken: cancellationToken);
        }
    }
}