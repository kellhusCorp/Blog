using System.ComponentModel.DataAnnotations;
using Blog.Domain.Entities;
using MediatR;

namespace Blog.Application.UseCases.AddPostComment
{
    public class AddPostCommentCommand : IRequest<Result<Unit>>
    {
        public string Link { get; set; }
        
        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        [StringLength(50)]
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string Body { get; set; }
        
        [StringLength(100)]
        [Required]
        public string Homepage { get; set; }
    }
}