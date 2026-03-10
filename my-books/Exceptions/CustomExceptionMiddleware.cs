using my_books.Data.ViewModels;
using System.Net;

namespace my_books.Exceptions
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionMiddleware(RequestDelegate next)
        {
                _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "application/json";

            var response = new ErrorViewModel()
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = "An Internal Server Error from Custom middleware",
                Path = "path-goes-here"
            };

            return httpContext.Response.WriteAsync(response.ToString());
        }
    }
}
