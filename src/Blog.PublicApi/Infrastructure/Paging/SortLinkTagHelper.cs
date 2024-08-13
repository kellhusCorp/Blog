using Microsoft.AspNetCore.Razor.TagHelpers;
using MyBlogOnCore.Infrastructure;

namespace Blog.PublicApi.Infrastructure.Paging;

[HtmlTargetElement("th", Attributes = "sort-column,paged-result")]
public class SortLinkTagHelper : TagHelper
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public SortLinkTagHelper(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public dynamic? PagedResult { get; set; }

    public string? SortColumn { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (PagedResult == null)
        {
            throw new InvalidOperationException($"{nameof(PagedResult)} не может быть null");
        }

        if (SortColumn == null)
        {
            throw new InvalidOperationException("'SortColumn' must be not null.");
        }

        string currentContent = (await output.GetChildContentAsync()).GetContent();

        string? url = httpContextAccessor.HttpContext?.Request.QueryString.Value;
        url = url.SetParameters(KeyValuePair.Create("skip", "0"));
        url = url.SetParameters(KeyValuePair.Create(nameof(Paging<object>.SortColumn), SortColumn));

        if (PagedResult.Paging.SortColumn == this.SortColumn)
        {
            if (PagedResult.Paging.SortDirection == SortDirection.Ascending)
            {
                url = url.SetParameters(KeyValuePair.Create(nameof(Paging<object>.SortDirection), SortDirection.Descending.ToString()));
                output.Content.SetHtmlContent($"<a href=\"{url}\">{currentContent}</a><i class=\"fa fa-caret-down text-danger d-print-none ml-1\"></i>");
            }
            else
            {
                url = url.SetParameters(KeyValuePair.Create(nameof(Paging<object>.SortDirection), SortDirection.Ascending.ToString()));
                output.Content.SetHtmlContent($"<a href=\"{url}\">{currentContent}</a><i class=\"fa fa-caret-up text-danger d-print-none ml-1\"></i>");
            }
        }
        else
        {
            url = url.SetParameters(KeyValuePair.Create(nameof(Paging<object>.SortDirection), SortDirection.Ascending.ToString()));
            output.Content.SetHtmlContent($"<a href=\"{url}\">{currentContent}</i></a><i class=\"fa fa-caret-down d-print-none ml-1\"></i>");
        }
    }
}