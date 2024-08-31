using AutoMapper;
using Blog.BLL.Dtos;
using Blog.Domain;
using Blog.Domain.Entities;

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