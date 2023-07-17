using API.Filters.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var error = new Error
            {
                StatusCode = "500",
                Message = context.Exception.Message
            };

            context.Result = new JsonResult(error) { StatusCode = 500, Value = error.Message };
        }
    }
}
