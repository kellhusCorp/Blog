using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBlogOnCore.BLL.Dtos;
using MyBlogOnCore.BLL.Queries;
using MyBlogOnCore.BLL.Repositories;
using PetBlog.Infrastructure.Types;

namespace MyBlogOnCore.BLL.Handlers
{
    public class GetPagesHandler : IRequestHandler<GetPagesQuery, OperationResult<IEnumerable<PageDto>>>
    {
        private readonly IPagesRepository _pagesRepository;
        private readonly IMapper _mapper;

        public GetPagesHandler(IPagesRepository pagesRepository, IMapper mapper)
        {
            _pagesRepository = pagesRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<IEnumerable<PageDto>>> Handle(GetPagesQuery request,
            CancellationToken cancellationToken)
        {
            var result = new OperationResult<IEnumerable<PageDto>>();

            result.Value = _mapper.Map<IEnumerable<PageDto>>(await _pagesRepository.GetAll().AsNoTracking()
                .ToListAsync(cancellationToken: cancellationToken));

            return result;
        }
    }
}