using Product.Microservice.Core.Domain.Result;
using Product.Microservice.Core.Enums;
using System.Text.Json;

namespace Product.Microservice.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate Next;
        private readonly ILogger<ExceptionMiddleware> Logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            Next = next;
            Logger = logger;
        }

        /// <summary>
        /// Handle all error Globally 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await Next(context);
            }
            catch (Exception ex)
            {
                var result = new Result<bool>
                {
                    Data = false,
                    StatusCode = StatusCode.InternalError,
                    ErrorMessages = new List<string> { ex.Message },
                };
                Logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(result, options);
                await context.Response.WriteAsync(json);

            }
        }
    }
}
