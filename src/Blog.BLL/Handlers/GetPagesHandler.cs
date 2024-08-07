using AutoMapper;
using Blog.BLL.Dtos;
using Blog.BLL.Queries;
using Blog.BLL.Repositories;
using Blog.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.BLL.Handlers
{
    public class GetPagesHandler : IRequestHandler<GetPagesQuery, Result<IEnumerable<PageDto>>>
    {
        private readonly IPagesRepository _pagesRepository;
        private readonly IMapper _mapper;

        public GetPagesHandler(IPagesRepository pagesRepository, IMapper mapper)
        {
            _pagesRepository = pagesRepository;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<PageDto>>> Handle(GetPagesQuery request,
            CancellationToken cancellationToken)
        {
            var result = new Result<IEnumerable<PageDto>>();

            result.Value = _mapper.Map<IEnumerable<PageDto>>(await _pagesRepository.GetAll().AsNoTracking()
                .ToListAsync(cancellationToken: cancellationToken));

            return result;
        }
    }
}