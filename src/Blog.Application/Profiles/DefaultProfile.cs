using AutoMapper;
using Blog.Application.UseCases.GetPages;
using Blog.Domain;

namespace Blog.Application.Profiles
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            CreateMap<Page, PageMetadataDto>();
        }
    }
}