using Blog.BLL.Dtos;
using Blog.BLL.Queries;
using Blog.BLL.Repositories;
using Blog.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.BLL.Handlers
{
    public class GetPagesMetadataHandler : IRequestHandler<GetPagesMetadataQuery, Result<IEnumerable<PageMetadataDto>>>
    {
        private readonly IPagesRepository _pagesRepository;

        public GetPagesMetadataHandler(IPagesRepository pagesRepository)
        {
            _pagesRepository = pagesRepository;
        }

        public Task<Result<IEnumerable<PageMetadataDto>>> Handle(GetPagesMetadataQuery request, CancellationToken cancellationToken)
        {
            var result = new Result<IEnumerable<PageMetadataDto>>();

            var metadata = _pagesRepository.GetAll().AsNoTracking()
                .Select(x => new PageMetadataDto(x.Name, x.Title));
            
            result.Value = metadata;

            return Task.FromResult(result);
        }
    }
}