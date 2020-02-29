using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IdentityManagement.Filters
{
    public class SecurityHeadersAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var result = context.Result;
            bool isViewResult = result is ViewResult;
            if (!isViewResult)
            {
                return;
            }
            if (!context.HttpContext.Response.Headers.ContainsKey("X-Content-Type-Options"))
            {
                context.HttpContext.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            }
            if (!context.HttpContext.Response.Headers.ContainsKey("X-Frame-Options"))
            {
                context.HttpContext.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
            }
            var referrer_policy = "no-referrer";
            if (!context.HttpContext.Response.Headers.ContainsKey("Referrer-Policy"))
            {
                context.HttpContext.Response.Headers.Add("Referrer-Policy", referrer_policy);
            }
        }
    }
}