using AutoMapper;
using Blog.BLL.Dtos;
using Blog.Domain;

namespace MyBlogOnCore.Profiles
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