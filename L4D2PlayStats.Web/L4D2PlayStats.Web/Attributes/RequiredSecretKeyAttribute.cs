using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace L4D2PlayStats.Web.Attributes;

public class RequiredSecretKeyAttribute : Attribute, IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var secretKey = configuration.GetValue<string>("SecretKey");

        if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var authorization))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (!string.Equals(authorization, secretKey, StringComparison.Ordinal))
            context.Result = new UnauthorizedResult();
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}