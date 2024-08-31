using AutoMapper;
using Blog.Application.Dtos;
using Blog.Application.UseCases.GetPages;
using Blog.Application.UseCases.GetPostByLink;
using Blog.Domain;
using Blog.Domain.Entities;

namespace Blog.Application.Profiles
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            CreateMap<Page, PageMetadataDto>();
            CreateMap<Post, PostDto>();
            CreateMap<TagAssignment, TagAssignmentDto>()
                .ForMember(x => x.PostId, opt => opt.MapFrom(x => x.BlogId));
            CreateMap<Comment, CommentDto>()
                .ForMember(x => x.PostId, opt => opt.MapFrom(x => x.BlogId));
            CreateMap<PostFile, PostFileDto>()
                .ForMember(x => x.PostId, opt => opt.MapFrom(x => x.BlogId));
            CreateMap<User, AuthorDto>();
            CreateMap<Tag, TagDto>();
        }
    }
}