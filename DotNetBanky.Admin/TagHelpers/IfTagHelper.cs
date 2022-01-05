using Microsoft.AspNetCore.Razor.TagHelpers;

namespace DotNetBanky.Admin.TagHelpers
{
    public static class Constants
    {
        public const string IncludeIfAttributeName = "include-if";
        public const string ExcludeIfAttributeName = "exclude-if";
    }

    [HtmlTargetElement(Attributes = Constants.IncludeIfAttributeName)]
    [HtmlTargetElement(Attributes = Constants.ExcludeIfAttributeName)]
    public class IfAttributeTagHelper : TagHelper
    {

        public override int Order => -1000;

        [HtmlAttributeName(Constants.IncludeIfAttributeName)]
        public bool? Include { get; set; }


        [HtmlAttributeName(Constants.ExcludeIfAttributeName)]
        public bool Exclude { get; set; } = false;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            output.Attributes.RemoveAll(Constants.IncludeIfAttributeName);
            output.Attributes.RemoveAll(Constants.ExcludeIfAttributeName);

            if (DontRender)
            {
                //TODO: make this an option?
                output.TagName = null;
                output.SuppressOutput();
            }
        }

        private bool DontRender => Exclude || (Include.HasValue && !Include.Value);
    }
}