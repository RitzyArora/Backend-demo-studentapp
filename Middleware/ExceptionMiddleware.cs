using StudentCrudAppWithEFCoreCodeFirst.Services;
using System.Net;
using System.Text.Json;

namespace StudentCrudAppWithEFCoreCodeFirst.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAppLogger _logger;

        public ExceptionMiddleware(RequestDelegate next,IAppLogger logger)
        {
            _next=next;
            _logger=logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);//passing request/response to next middleware
            }
            catch (Exception ex) 
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            _logger.Log($"Error : {ex.Message}");

            //set Response
            context.Response.ContentType = "application/json";
            context.Response.StatusCode=(int)HttpStatusCode.InternalServerError;

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message="Something went wrong please try again later!!",
                Detailed=ex.Message
            };
             await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
