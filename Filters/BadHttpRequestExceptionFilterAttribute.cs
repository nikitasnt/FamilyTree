using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FamilyTree.Filters;

/// <summary>
/// Exception filter <see cref="BadHttpRequestException"/>. Write the exception message to the response body.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class BadHttpRequestExceptionFilterAttribute : Attribute, IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is BadHttpRequestException e)
        {
            context.Result = new BadRequestObjectResult(e.Message)
            {
                StatusCode = e.StatusCode
            };
            context.ExceptionHandled = true;
        }
        else
        {
            context.ExceptionHandled = false;
        }
    }
}