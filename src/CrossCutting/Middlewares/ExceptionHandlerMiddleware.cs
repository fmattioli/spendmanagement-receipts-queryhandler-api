using Application.Validators;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using System.Net;

namespace CrossCutting.Middlewares
{
    public class ExceptionHandlerMiddleware : AbstractExceptionHandlerMiddleware
    {
        private readonly ILogger _logger;
        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger logger) : base(next)
        {
            _logger = logger;
        }

        public override (HttpStatusCode code, string message) GetResponse(Exception exception)
        {
            var code = exception switch
            {
                NotFoundException => HttpStatusCode.NotFound,
                _ => HttpStatusCode.InternalServerError,
            };

            _logger.Error(exception, "The previosly error occurred. ");

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
