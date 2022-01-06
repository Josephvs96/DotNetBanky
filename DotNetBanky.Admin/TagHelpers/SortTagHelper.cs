using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace DotNetBanky.Admin.TagHelpers
{
    public class SortTagHelper : TagHelper
    {
        private enum SortButton
        {
            asc, desc
        }

        public Enum SortColumn { get; set; } = null!;

        [ViewContext]
        public ViewContext ViewContext { get; set; } = null!;


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var container = new TagBuilder("div");
            container.AddCssClass("d-inline");
            container.InnerHtml.AppendHtml(CreateSortingLink(SortButton.asc));
            container.InnerHtml.AppendHtml(CreateSortingLink(SortButton.desc));

            output.Content.SetHtmlContent(container);
        }

        private TagBuilder CreateSortingLink(SortButton sortButton)
        {
            var pageUrl = CreateUrlTemplate(sortButton);

            var iTag = new TagBuilder("i");
            iTag.AddCssClass(
                sortButton switch
                {
                    SortButton.asc => "fa fa-angle-up",
                    SortButton.desc => "fa fa-angle-down"
                });

            var aTag = new TagBuilder("a");
            aTag.AddCssClass("btn p-0");
            aTag.Attributes.Add("href", pageUrl);


            aTag.InnerHtml.AppendHtml(iTag);
            return aTag;
        }

        private string CreateUrlTemplate(SortButton sortButton)
        {
            var queryString = ViewContext.HttpContext.Request.QueryString.Value;

            const string queryStringKeySortColumn = "sortColumn";
            const string queryStringKeySortDirection = "sortDirection";


            var urlTemplate = string.IsNullOrWhiteSpace(queryString)
                ? $"{queryStringKeySortColumn}={SortColumn}&{queryStringKeySortDirection}={sortButton}".Split('&').ToList()
                : queryString.TrimStart('?').Split('&').ToList();

            var qDic = new List<Tuple<string, object>>
            {
                new Tuple<string, object>(queryStringKeySortColumn, "{0}"),
                new Tuple<string, object>(queryStringKeySortDirection, "{1}")
            };

            var excludedKeys = new List<string> { "X-Requested-With", "_", "sortColumn", "sortDirection" };

            foreach (var item in urlTemplate)
            {
                var key = item.Split('=')[0];
                var value = item.Split('=')[1];

                if (!excludedKeys.Contains(key))
                {
                    qDic.Add(new Tuple<string, object>(key, value));
                }
            }

            var path = ViewContext.HttpContext.Request.Path;
            var pathWithParams = path + "?" + string.Join("&", qDic.Select(q => q.Item1 + "=" + q.Item2));
            return string.Format(pathWithParams, SortColumn, sortButton);
        }
    }
}
