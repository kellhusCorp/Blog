using AutoMapper;
using Blog.BLL.Dtos;
using Blog.Domain;

namespace Blog.PublicApi.Profiles
{
    public class PageProfile : Profile
    {
        public PageProfile()
        {
            CreateMap<Page, PageDto>()
                .ReverseMap();
        }
    }
}