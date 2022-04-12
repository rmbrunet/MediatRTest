using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace MediatRTest.Common
{
    public class ActionValidationFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No needed
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                if (context.HttpContext.Request.Method == "GET")
                {
                    var result = new BadRequestResult();
                    context.Result = result;
                }
                else
                {
                    var result = new ContentResult();

                    var content = System.Text.Json.JsonSerializer.Serialize(context.ModelState,
                        new JsonSerializerOptions
                        {
                        });

                    result.Content = content;
                    result.ContentType = "application/json";

                    context.HttpContext.Response.StatusCode = 400;
                    context.Result = result;
                }
            }
        }
    }
}