using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blog.Application.Contexts;
using Blog.Domain;
using Blog.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.UseCases.GetPages
{
    public class GetPagesMetadataHandler(IDbContext dbContext, IMapper mapper)
        : IRequestHandler<GetPagesMetadataQuery, Result<IEnumerable<PageMetadataDto>>>
    {
        public async Task<Result<IEnumerable<PageMetadataDto>>> Handle(GetPagesMetadataQuery request, CancellationToken cancellationToken)
        {
            var metadata = await dbContext.Pages
                .AsNoTracking()
                .ProjectTo<PageMetadataDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            
            return Result<IEnumerable<PageMetadataDto>>.Success(metadata);
        }
    }
}