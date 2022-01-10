using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace DotNetBanky.Admin.TagHelpers
{
    public class PagingTagHelper : TagHelper
    {
        private IConfiguration Configuration { get; }
        private readonly ILogger _logger;

        private string UrlTemplate { get; set; }

        [ViewContext]
        public ViewContext ViewContext { get; set; } = null;

        public PagingTagHelper(IConfiguration configuration, ILogger<PagingTagHelper> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        #region Settings

        /// <summary>
        /// current page number.
        /// <para>default: 1</para>
        /// <para>example: p=1</para>
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// how many items to get from db per page per request
        /// <para>default: 10</para>
        /// <para>example: pageSize=10</para>
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Total count of records in the db
        /// <para>default: 0</para>
        /// </summary>
        public int TotalRecords { get; set; } = 0;

        /// <summary>
        /// if count of pages is too much, restrict shown pages count to specific number
        /// <para>default: 10</para>
        /// </summary>
        public int? MaxDisplayedPages { get; set; }

        /// <summary>
        /// Force adding url path to the navigation url
        /// in some scenarios when the page is under some area/subFolder
        /// The navigation links are pointing to the home page.
        /// To force adding url path enable this property
        /// </summary>
        public bool? FixUrlPath { get; set; } = true;

        #endregion

        #region Page size navigation
        /// <summary>
        /// A list of dash delimitted numbers for page size dropdown. 
        /// default: "10-25-50"
        /// </summary>
        public string PageSizeDropdownItems { get; set; }

        #endregion

        #region QueryString

        /// <summary>
        /// Query string paramter name for current page.
        /// <para>default: p</para>
        /// <para>exmaple: p=1</para>
        /// </summary>
        public string QueryStringKeyPageNo { get; set; }

        /// <summary>
        /// Query string parameter name for page size
        /// <para>default: s</para>
        /// <para>example: s=25</para>
        /// </summary>
        public string QueryStringKeyPageSize { get; set; }

        #endregion

        #region Display settings

        /// <summary>
        /// Show drop down list for different page size options
        /// <para>default: true</para>
        /// <para>options: true, false</para>
        /// </summary>
        public bool? ShowPageSizeNav { get; set; }

        /// <summary>
        /// Show a three dots after first page or before last page 
        /// when there is a gap in pages at the beginnig or end
        /// </summary>
        public bool? ShowGap { get; set; }

        /// <summary>
        /// Show/hide First-Last buttons
        /// <para>default: true, if set to false and total pages > max displayed pages it will be true</para>
        /// </summary>
        public bool? ShowFirstLast { get; set; }

        /// <summary>
        /// Show/hide Previous-Next buttons
        /// <para>default: true</para>
        /// </summary>
        public bool? ShowPrevNext { get; set; }
        #endregion

        #region Texts
        /// <summary>
        /// The text to display at page size dropdown list label
        /// <para>default: Page size </para>
        /// </summary>
        public string TextPageSize { get; set; }
        /// <summary>
        /// Text to show on the "Go To First" Page button
        /// <para>
        /// <![CDATA[default: &laquo;]]></para>
        /// </summary>
        public string TextFirst { get; set; }

        /// <summary>
        /// Text to show on "Go to last page" button
        /// <para>
        /// <![CDATA[default: &raquo;]]></para>
        /// </summary>
        public string TextLast { get; set; }

        /// <summary>
        /// Next button text
        /// <para>
        /// <![CDATA[default: &rsaquo;]]></para>
        /// </summary>
        public string TextNext { get; set; }

        /// <summary>
        /// previous button text
        /// <para>
        /// <![CDATA[default: &lsaquo;]]></para>
        /// </summary>
        public string TextPrevious { get; set; }
        #endregion

        #region Styling

        /// <summary>
        /// add custom class to content div
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// css class for pagination div
        /// </summary>
        public string ClassPagingControlDiv { get; set; }

        /// <summary>
        /// css class for page count/record count div
        /// </summary>
        public string ClassInfoDiv { get; set; }

        /// <summary>
        /// styling class for page size div
        /// </summary>
        public string ClassPageSizeDiv { get; set; }

        /// <summary>
        /// pagination control class
        /// <para>default: pagination</para>
        /// </summary>
        public string ClassPagingControl { get; set; }

        /// <summary>
        /// class name for the active page
        /// <para>default: active</para>
        /// <para>examples: disabled, active, ...</para>
        /// </summary>
        public string ClassActivePage { get; set; }

        /// <summary>
        /// name of the class when jumping button is disabled.
        /// jumping buttons are prev-next and first-last buttons
        /// <param>default: disabled</param>
        /// <para>example: disabled, d-hidden</para>
        /// </summary>
        public string ClassDisabledJumpingButton { get; set; }

        /// <summary>
        /// css class for total records info
        /// <para>default: badge badge-light</para>
        /// </summary>
        public string ClassTotalRecords { get; set; }

        /// <summary>
        /// css class for total pages info
        /// <para>default: badge badge-light</para>
        /// </summary>
        public string ClassTotalPages { get; set; }

        /// <summary>
        /// css class for page link, use for styling bg and text colors
        /// </summary>
        public string ClassPageLink { get; set; }

        #endregion


        private int TotalPages => (int)Math.Ceiling(TotalRecords / (double)PageSize);

        private class Boundaries
        {
            public int Start { get; set; }
            public int End { get; set; }
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            SetDefaults();

            if (TotalPages > 0)
            {
                var pagingControl = new TagBuilder("ul");
                pagingControl.AddCssClass($"{ClassPagingControl}");

                // show-hide first-last buttons on user options
                if (ShowFirstLast == true)
                {
                    ShowFirstLast = true;
                }

                UrlTemplate = CreatePagingUrlTemplate();

                if (ShowFirstLast == true)
                {
                    var first = CreatePagingLink(1, TextFirst, "First Page", ClassDisabledJumpingButton);
                    pagingControl.InnerHtml.AppendHtml(first);
                }

                if (ShowPrevNext == true)
                {
                    var prevPage = PageNumber - 1 <= 1 ? 1 : PageNumber - 1;
                    var prev = CreatePagingLink(prevPage, TextPrevious, "Previous Page", ClassDisabledJumpingButton);
                    pagingControl.InnerHtml.AppendHtml(prev);
                }

                if (MaxDisplayedPages == 1)
                {
                    var numTag = CreatePagingLink(PageNumber, null, null, ClassActivePage);
                    pagingControl.InnerHtml.AppendHtml(numTag);
                }
                else if (MaxDisplayedPages > 1)
                {
                    // Boundaries are the start-end currently displayed pages
                    var boundaries = CalculateBoundaries(PageNumber, TotalPages, MaxDisplayedPages.Value);

                    string gapStr = "<li class=\"page-item border-0\">&nbsp;...&nbsp;</li>";

                    if (ShowGap == true && boundaries.End > MaxDisplayedPages)
                    {
                        // add page no 1
                        var num1Tag = CreatePagingLink(1, null, null, ClassActivePage);
                        pagingControl.InnerHtml.AppendHtml(num1Tag);

                        // Add gap after first page
                        pagingControl.InnerHtml.AppendHtml(gapStr);
                    }

                    for (int i = boundaries.Start; i <= boundaries.End; i++)
                    {
                        var numTag = CreatePagingLink(i, null, null, ClassActivePage);
                        pagingControl.InnerHtml.AppendHtml(numTag);
                    }

                    if (ShowGap == true && boundaries.End < TotalPages)
                    {
                        // Add gap before last page
                        pagingControl.InnerHtml.AppendHtml(gapStr);

                        // add last page
                        var numLastTag = CreatePagingLink(TotalPages, null, null, ClassActivePage);
                        pagingControl.InnerHtml.AppendHtml(numLastTag);
                    }
                }

                if (ShowPrevNext == true)
                {
                    var nextPage = PageNumber + 1 > TotalPages ? TotalPages : PageNumber + 1;
                    var next = CreatePagingLink(nextPage, TextNext, "Next Page", ClassDisabledJumpingButton);
                    pagingControl.InnerHtml.AppendHtml(next);
                }

                if (ShowFirstLast == true)
                {
                    var last = CreatePagingLink(TotalPages, TextLast, "Last Page", ClassDisabledJumpingButton);
                    pagingControl.InnerHtml.AppendHtml(last);
                }

                var pagingControlDiv = new TagBuilder("div");
                pagingControlDiv.AddCssClass($"{ClassPagingControlDiv}");
                pagingControlDiv.InnerHtml.AppendHtml(pagingControl);

                output.TagName = "div";
                output.Attributes.SetAttribute("class", $"{Class}");
                output.Content.AppendHtml(pagingControlDiv);

                if (ShowPageSizeNav == true)
                {
                    var psDropdown = CreatePageSizeControl();

                    var psDiv = new TagBuilder("div");
                    psDiv.AddCssClass($"{ClassPageSizeDiv}");
                    psDiv.InnerHtml.AppendHtml(psDropdown);

                    output.Content.AppendHtml(psDiv);
                }
            }
        }

        private void SetDefaults()
        {

            MaxDisplayedPages = MaxDisplayedPages == null ? 5 : MaxDisplayedPages;

            PageSizeDropdownItems = PageSizeDropdownItems ?? "10-25-50";

            QueryStringKeyPageNo = QueryStringKeyPageNo ?? "pageNumber";

            QueryStringKeyPageSize = QueryStringKeyPageSize ?? "pageSize";

            ShowGap ??= true;

            ShowFirstLast ??= true;

            ShowPrevNext ??= true;

            ShowPageSizeNav ??= true;

            TextFirst = TextFirst ?? "&laquo;";

            TextLast = TextLast ?? "&raquo;";

            TextPrevious = TextPrevious ?? "&lsaquo;";

            TextNext = TextNext ?? "&rsaquo;";

            Class = Class ?? "row";

            ClassActivePage = ClassActivePage ?? "active";

            ClassDisabledJumpingButton = ClassDisabledJumpingButton ?? "disabled";

            ClassInfoDiv = ClassInfoDiv ?? "col-2";

            ClassPageSizeDiv = ClassPageSizeDiv ?? "col-1";

            ClassPagingControlDiv = ClassPagingControlDiv ?? "col";

            ClassPagingControl = ClassPagingControl ?? "pagination";

            ClassTotalPages = ClassTotalPages ?? "badge bg-light text-dark";

            ClassTotalRecords = ClassTotalRecords ?? "badge bg-dark";

            ClassPageLink = ClassPageLink ?? "";

            FixUrlPath ??= true;

            _logger.LogInformation($"----> PagingTagHelper - " +
                $"{nameof(PageNumber)}: {PageNumber}, " +
                $"{nameof(PageSize)}: {PageSize}, " +
                $"{nameof(TotalRecords)}: {TotalRecords}, " +
                $"{nameof(TotalPages)}: {TotalPages}, " +
                $"{nameof(QueryStringKeyPageNo)}: {QueryStringKeyPageNo}, " +
                $"{nameof(QueryStringKeyPageSize)}: {QueryStringKeyPageSize}, " +
                $"");
        }

        private Boundaries CalculateBoundaries(int currentPageNo, int totalPages, int maxDisplayedPages)
        {
            int _start, _end;

            int _gap = (int)Math.Ceiling(maxDisplayedPages / 2.0);

            if (maxDisplayedPages > totalPages)
                maxDisplayedPages = totalPages;

            if (totalPages == 1)
            {
                _start = _end = 1;
            }
            // << < 1 2 (3) 4 5 6 7 8 9 10 > >>
            else if (currentPageNo < maxDisplayedPages)
            {
                _start = 1;
                _end = maxDisplayedPages;
            }
            // << < 91 92 93 94 95 96 97 (98) 99 100 > >>
            else if (currentPageNo + maxDisplayedPages == totalPages)
            {
                _start = totalPages - maxDisplayedPages > 0 ? totalPages - maxDisplayedPages - 1 : 1;
                _end = totalPages - 2;
            }
            // << < 91 92 93 94 95 96 97 (98) 99 100 > >>
            else if (currentPageNo + maxDisplayedPages == totalPages + 1)
            {
                _start = totalPages - maxDisplayedPages > 0 ? totalPages - maxDisplayedPages : 1;
                _end = totalPages - 1;
            }
            // << < 91 92 93 94 95 96 97 (98) 99 100 > >>
            else if (currentPageNo + maxDisplayedPages > totalPages + 1)
            {
                _start = totalPages - maxDisplayedPages > 0 ? totalPages - maxDisplayedPages + 1 : 1;
                _end = totalPages;
            }

            // << < 21 22 23 34 (25) 26 27 28 29 30 > >>
            else
            {
                _start = currentPageNo - _gap > 0 ? currentPageNo - _gap + 1 : 1;
                _end = _start + maxDisplayedPages - 1;
            }

            return new Boundaries { Start = _start, End = _end };
        }

        private TagBuilder CreatePagingLink(int targetPageNo, string text, string textSr, string pClass)
        {
            var liTag = new TagBuilder("li");
            liTag.AddCssClass("page-item");
            liTag.Attributes.Add("title", textSr);

            var pageUrl = CreatePagingUrl(targetPageNo, PageSize);

            var aTag = new TagBuilder("a");
            aTag.AddCssClass($"page-link {ClassPageLink}");
            aTag.Attributes.Add("href", pageUrl);

            // If no text provided for screen readers
            // use the actual page number
            if (string.IsNullOrWhiteSpace(textSr))
            {
                var pageNoText = targetPageNo;

                aTag.InnerHtml.Append($"{pageNoText}");
            }
            else
            {
                liTag.MergeAttribute("area-label", textSr);
                aTag.InnerHtml.AppendHtml($"<span area-hidden=\"true\">{text}</span>");
                aTag.InnerHtml.AppendHtml($"<span class=\"visually-hidden-focusable\">{textSr}</span>");

            }

            if (PageNumber == targetPageNo)
            {
                liTag.AddCssClass($"{pClass}");
                aTag.Attributes.Add("tabindex", "-1");
                aTag.Attributes.Remove("class");
                aTag.AddCssClass($"page-link {pClass}");
                aTag.Attributes.Remove("href");
            }

            liTag.InnerHtml.AppendHtml(aTag);
            return liTag;
        }

        private TagBuilder CreatePageSizeControl()
        {
            var dropDownDiv = new TagBuilder("div");
            dropDownDiv.AddCssClass("dropdown");
            dropDownDiv.Attributes.Add("title", "Number of items to show");

            var dropDownBtn = new TagBuilder("button");
            dropDownBtn.AddCssClass("btn page-link dropdown-toggle");
            dropDownBtn.Attributes.Add("type", "button");
            dropDownBtn.Attributes.Add("id", "pagingDropDownMenuBtn");


            dropDownBtn.Attributes.Add("data-bs-toggle", "dropdown");

            dropDownBtn.Attributes.Add("aria-haspopup", "true");
            dropDownBtn.Attributes.Add("aria-expanded", "false");

            var psText = string.IsNullOrWhiteSpace(TextPageSize)
                ? $"{PageSize}"
                : string.Format(TextPageSize, $"{PageSize}");
            dropDownBtn.InnerHtml.Append(psText);

            var dropDownMenu = new TagBuilder("div");
            dropDownMenu.AddCssClass("dropdown-menu dropdown-menu-right");
            dropDownMenu.Attributes.Add("aria-labelledby", "pagingDropDownMenuBtn");

            var pageSizeDropdownItems = PageSizeDropdownItems.Split("-", StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < pageSizeDropdownItems.Length; i++)
            {
                var n = int.Parse(pageSizeDropdownItems[i]);

                var pageUrl = $"{CreatePagingUrl(1, n)}";

                var option = new TagBuilder("a");
                option.AddCssClass("dropdown-item");
                option.Attributes.Add("href", pageUrl);

                option.InnerHtml.Append($"{n}");

                if (n == PageSize)
                    option.AddCssClass("active");

                dropDownMenu.InnerHtml.AppendHtml(option);
            }

            dropDownDiv.InnerHtml.AppendHtml(dropDownBtn);
            dropDownDiv.InnerHtml.AppendHtml(dropDownMenu);

            return dropDownDiv;
        }

        private string CreatePagingUrl(int pageNo, int pageSize)
        {
            return string.Format(UrlTemplate, pageNo, pageSize);
        }

        private string CreatePagingUrlTemplate()
        {
            var queryString = ViewContext.HttpContext.Request.QueryString.Value;

            var urlTemplate = string.IsNullOrWhiteSpace(queryString)
                ? $"{QueryStringKeyPageNo}=1&{QueryStringKeyPageSize}={PageSize}".Split('&').ToList()
                : queryString.TrimStart('?').Split('&').ToList();

            var qDic = new List<Tuple<string, object>>
            {
                new Tuple<string, object>(QueryStringKeyPageNo, "{0}"),
                new Tuple<string, object>(QueryStringKeyPageSize, "{1}")
            };

            var excludedKeys = new List<string> { "X-Requested-With", "_", QueryStringKeyPageNo, QueryStringKeyPageSize };
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

            return FixUrlPath ?? true
                ? path + "?" + string.Join("&", qDic.Select(q => q.Item1 + "=" + q.Item2))
                : "?" + string.Join("&", qDic.Select(q => q.Item1 + "=" + q.Item2));
        }
    }
}
