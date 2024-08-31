using Blog.Domain;
using Blog.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Blog.PublicApi.Extensions
{
    public static class ResultExtensions
    {
        public static IActionResult AsOk<T>(this Result<T> result)
        {
            if (result.IsSuccessful)
            {
                return new OkObjectResult(result.Value);
            }
            
            return new BadRequestObjectResult(result.ErrorMessage);
        }
    }
}