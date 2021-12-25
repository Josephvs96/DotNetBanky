using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DotNetBanky.Core.Filters
{
    public class NoAccessAttribute : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            context.Result = new LocalRedirectResult("~/");
        }

    }
}
