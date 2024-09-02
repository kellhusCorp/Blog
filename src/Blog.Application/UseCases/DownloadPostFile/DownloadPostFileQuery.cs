using System.ComponentModel.DataAnnotations;
using Blog.Domain;
using Blog.Domain.Entities;
using MediatR;

namespace Blog.Application.UseCases.DownloadPostFile
{
    public class DownloadPostFileQuery : IRequest<Result<DownloadPostFileResponse>>
    {
        [Required]
        public Guid PostFileId { get; }

        public DownloadPostFileQuery(Guid postFileId)
        {
            PostFileId = postFileId;
        }
    }
}