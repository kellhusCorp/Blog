using AutoMapper;

namespace MyBlogOnCore.Profiles
{
    public class PageProfile : Profile
    {
        public PageProfile()
        {
            CreateMap<Domain.Page, BLL.Dtos.PageDto>()
                .ReverseMap();
        }
    }
}