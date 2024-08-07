using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBlogOnCore.BLL.Dtos;
using MyBlogOnCore.BLL.Queries;
using MyBlogOnCore.BLL.Repositories;
using PetBlog.Infrastructure.Types;

namespace MyBlogOnCore.BLL.Handlers
{
    public class GetPagesMetadataHandler : IRequestHandler<GetPagesMetadataQuery, OperationResult<IEnumerable<PageMetadataDto>>>
    {
        private readonly IPagesRepository _pagesRepository;

        public GetPagesMetadataHandler(IPagesRepository pagesRepository)
        {
            _pagesRepository = pagesRepository;
        }

        public Task<OperationResult<IEnumerable<PageMetadataDto>>> Handle(GetPagesMetadataQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<IEnumerable<PageMetadataDto>>();

            var metadata = _pagesRepository.GetAll().AsNoTracking()
                .Select(x => new PageMetadataDto(x.Name, x.Title));
            
            result.Value = metadata;

            return Task.FromResult(result);
        }
    }
}