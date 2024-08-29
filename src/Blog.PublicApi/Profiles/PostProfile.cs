using AutoMapper;
using Blog.Application.UseCases.GetPostByLink;
using Blog.PublicApi.Models;

namespace Blog.PublicApi.Profiles
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<PostWithRelatedPostsDto, PostViewModel>();
        }
    }
}