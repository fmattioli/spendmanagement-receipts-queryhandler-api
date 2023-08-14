using Application.Validators;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;

namespace CrossCutting.Middlewares
{
    public class ExceptionHandlerMiddleware : AbstractExceptionHandlerMiddleware
    {
        public ExceptionHandlerMiddleware(RequestDelegate next) : base(next)
        {
        }

        public override (HttpStatusCode code, string message) GetResponse(Exception exception)
        {
            var code = exception switch
            {
                NotFoundException => HttpStatusCode.NotFound,
                _ => HttpStatusCode.InternalServerError,
            };

            return (code, JsonConvert.SerializeObject(new Error
            {
                StatusCode = code,
                Message = exception.Message,
            }));
        }

        public record Error
        {
            public HttpStatusCode StatusCode { get; set; }
            public string? Message { get; set; }
            public string? StackTrace { get; set; }
        }
    }
}
