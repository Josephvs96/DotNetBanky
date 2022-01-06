using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;

namespace DotNetBanky.Admin.TagHelpers
{
    [HtmlTargetElement("li", Attributes = _for)]
    public class ActiveItemTagHelper : TagHelper
    {
        private readonly IUrlHelper _urlHelper;
        private readonly IHttpContextAccessor _httpAccess;
        private readonly LinkGenerator _linkGenerator;
        private const string _for = "navigation-active-for";

        public ActiveItemTagHelper(
            IActionContextAccessor actionAccess,
            IUrlHelperFactory factory,
            IHttpContextAccessor httpAccess,
            LinkGenerator generator
        )
        {
            _urlHelper = factory.GetUrlHelper(actionAccess.ActionContext);
            _httpAccess = httpAccess;
            _linkGenerator = generator;
        }

        public async override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // grab attribute value
            var targetPage = output.Attributes[_for].Value.ToString();
            // remove from html so user doesn't see it
            output.Attributes.Remove(output.Attributes[_for]);

            // get the URI that corresponds to the attribute value
            var targetUri = _linkGenerator.GetUriByPage(_httpAccess.HttpContext, page: targetPage);
            // get the URI that corresponds to the current page's action
            var currentUri = _httpAccess.HttpContext.Request.Path.Value;

            // if they match, then add the "active" CSS class
            if (currentUri == "/" && targetPage == "/")
            {
                output.AddClass("active", HtmlEncoder.Default);
                return;
            }

            if (currentUri.Split("/").Contains(targetPage))
            {
                output.AddClass("active", HtmlEncoder.Default);
            }
        }
    }

    [HtmlTargetElement("li", Attributes = _for)]
    public class SubItemTagHelper : TagHelper
    {
        private readonly IUrlHelper _urlHelper;
        private readonly IHttpContextAccessor _httpAccess;
        private readonly LinkGenerator _linkGenerator;
        private const string _for = "sub-item-active-for";

        public SubItemTagHelper(
            IActionContextAccessor actionAccess,
            IUrlHelperFactory factory,
            IHttpContextAccessor httpAccess,
            LinkGenerator generator
        )
        {
            _urlHelper = factory.GetUrlHelper(actionAccess.ActionContext);
            _httpAccess = httpAccess;
            _linkGenerator = generator;
        }

        public async override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // grab attribute value
            var targetPage = output.Attributes[_for].Value.ToString();
            // remove from html so user doesn't see it
            output.Attributes.Remove(output.Attributes[_for]);

            // get the URI that corresponds to the attribute value
            var targetUri = _linkGenerator.GetUriByPage(_httpAccess.HttpContext, page: targetPage);
            // get the URI that corresponds to the current page's action
            var currentUri = _urlHelper.ActionLink();

            // if they match, then add the "active" CSS class
            if (currentUri == targetUri)
            {
                output.AddClass("active", HtmlEncoder.Default);
            }
        }
    }

    [HtmlTargetElement(Attributes = _for)]
    public class ActiveMenuTagHelper : TagHelper
    {
        private readonly IUrlHelper _urlHelper;
        private readonly IHttpContextAccessor _httpAccess;
        private readonly LinkGenerator _linkGenerator;
        private const string _for = "menu-active-for";

        public ActiveMenuTagHelper(
            IActionContextAccessor actionAccess,
            IUrlHelperFactory factory,
            IHttpContextAccessor httpAccess,
            LinkGenerator generator
        )
        {
            _urlHelper = factory.GetUrlHelper(actionAccess.ActionContext);
            _httpAccess = httpAccess;
            _linkGenerator = generator;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string areaExpanded = null;

            if (output.TagName == "a")
            {
                areaExpanded = output.Attributes["aria-expanded"].Value.ToString();
            }
            // grab attribute value
            var targetPage = output.Attributes[_for].Value.ToString();
            // remove from html so user doesn't see it
            output.Attributes.Remove(output.Attributes[_for]);

            // get the URI that corresponds to the attribute value
            var targetUri = _linkGenerator.GetUriByPage(_httpAccess.HttpContext, page: targetPage);
            // get the URI that corresponds to the current page's action
            var currentUri = _httpAccess.HttpContext.Request.Path.Value;

            // if they match, then add the "active" CSS class
            if (currentUri == "/" && targetPage == "/")
            {
                output.AddClass("show", HtmlEncoder.Default);
                output.RemoveClass("collapsed", HtmlEncoder.Default);

                if (areaExpanded != null) output.Attributes.SetAttribute("aria-expanded", true);
                return;
            }

            if (currentUri!.Split("/").Contains(targetPage))
            {
                output.AddClass("show", HtmlEncoder.Default);
                output.RemoveClass("collapsed", HtmlEncoder.Default);
                if (areaExpanded != null) output.Attributes.SetAttribute("aria-expanded", true);
            }
        }

    }
}
