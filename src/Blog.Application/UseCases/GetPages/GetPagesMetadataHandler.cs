using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blog.Application.Contexts;
using Blog.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.UseCases.GetPages
{
    public class GetPagesMetadataHandler : IRequestHandler<GetPagesMetadataQuery, Result<IEnumerable<PageMetadataDto>>>
    {
        private readonly IDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetPagesMetadataHandler(IDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        
        public async Task<Result<IEnumerable<PageMetadataDto>>> Handle(GetPagesMetadataQuery request, CancellationToken cancellationToken)
        {
            var metadata = await _dbContext.Pages
                .AsNoTracking()
                .ProjectTo<PageMetadataDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            return Result<IEnumerable<PageMetadataDto>>.Success(metadata);
        }
    }
}