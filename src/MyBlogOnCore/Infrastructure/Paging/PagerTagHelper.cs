using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.WebUtilities;

namespace MyBlogOnCore.Infrastructure.Paging;

[HtmlTargetElement("pager")]
public class PagerTagHelper : TagHelper
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public PagerTagHelper(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public dynamic PagedResult { get; set; } = null!;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var listBuilder = new StringBuilder();

        int totalPages = (int)Math.Ceiling((double)PagedResult.TotalNumberOfItems / PagedResult.Paging.Top);

        var pagingIndexes = GetPagingIndexes(
            this.PagedResult.Paging.Skip / PagedResult.Paging.Top,
            totalPages);

        if (totalPages > 1)
        {
            listBuilder.AppendLine("<form class=\"form-inline\" method=\"get\">");
            listBuilder.AppendLine("<div class=\"input-group mr-sm-2\">");
            listBuilder.AppendLine("<ul class=\"pagination\">");

            for (int i = 0; i < pagingIndexes.Length; i++)
            {
                if (i > 0 && pagingIndexes[i - 1] != pagingIndexes[i] - 1)
                {
                    listBuilder.AppendLine("<li class=\"page-item disabled\"><a href=\"#\" class=\"page-link\">&hellip;</a></li>");
                }

                if ((PagedResult.Paging.Skip / PagedResult.Paging.Top) == pagingIndexes[i])
                {
                    listBuilder.AppendLine("<li class=\"page-item active\"><a href=\"#\" class=\"page-link\">" + (pagingIndexes[i] + 1) + "</a></li>");
                }
                else
                {
                    string? url = this.httpContextAccessor.HttpContext?.Request.QueryString.Value;
                    string skip = (pagingIndexes[i] * PagedResult.Paging.Top).ToString();
                    url = url.SetParameters(KeyValuePair.Create("skip", skip));

                    listBuilder.AppendLine("<li class=\"page-item\"><a href=\"" + url + "\" class=\"page-link\">" + (pagingIndexes[i] + 1) + "</a></li>");
                }
            }

            listBuilder.AppendLine("</ul>");
            listBuilder.AppendLine("</div>");

            if (PagedResult.Paging.Top >= 20)
            {
                var query = QueryHelpers.ParseQuery(this.httpContextAccessor.HttpContext!.Request.QueryString.Value!);

                foreach (var item in query)
                {
                    if (!item.Key.Equals("top", StringComparison.OrdinalIgnoreCase)
                        && !item.Key.Equals("skip", StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (var value in item.Value)
                        {
                            listBuilder.Append($"<input type=\"hidden\" name=\"{item.Key}\" value=\"{value}\" />");
                        }
                    }
                }

                listBuilder.AppendLine("<div class=\"input-group\">");
                listBuilder.AppendLine("<div class=\"input-group-prepend\">");
                listBuilder.AppendLine("<div class=\"input-group-text\"><i class=\"fa fa-book text-primary\"></i></div>");
                listBuilder.AppendLine("</div>");

                listBuilder.AppendLine("<select class=\"form-control\" name=\"top\" onchange=\"this.form.submit()\">");
                listBuilder.Append($"<option value=\"20\"{(PagedResult.Paging.Top == 20 ? " selected" : string.Empty)}>20</option>");
                listBuilder.Append($"<option value=\"50\"{(PagedResult.Paging.Top == 50 ? " selected" : string.Empty)}>50</option>");
                listBuilder.Append($"<option value=\"100\"{(PagedResult.Paging.Top == 100 ? " selected" : string.Empty)}>100</option>");
                listBuilder.Append($"<option value=\"200\"{(PagedResult.Paging.Top == 200 ? " selected" : string.Empty)}>200</option>");
                listBuilder.AppendLine("</select>");
                listBuilder.AppendLine("</div>");
            }

            listBuilder.AppendLine("</form>");
        }

        output.TagName = "div";
        output.Content.SetHtmlContent(listBuilder.ToString());

        output.PreElement.SetHtmlContent("<nav class=\"d-print-none\">");
        output.PostElement.SetHtmlContent("</nav>");
    }

    private static int[] GetPagingIndexes(int currentIndex, int totalPages)
    {
        var result = new HashSet<int>();

        for (int i = 0; i < 2; i++)
        {
            if (i <= totalPages)
            {
                result.Add(i);
            }
        }

        int current = currentIndex - 2;

        while (current <= currentIndex + 2)
        {
            if (current > 0 && current < totalPages)
            {
                result.Add(current);
            }

            current++;
        }

        for (int i = totalPages - 2; i < totalPages; i++)
        {
            result.Add(i);
        }

        return result.ToArray();
    }
}
