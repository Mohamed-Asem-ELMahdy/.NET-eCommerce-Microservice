using Microsoft.AspNetCore.Http;

namespace eCommerce.SharedLibrary.Middleware;

public class ApiGateway(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext request)
    {
        var header = request.Request.Headers["Api-Gateway"];
        if (header.FirstOrDefault() is null)
        {
            request.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            await request.Response.WriteAsync("Api Gateway is not available");
            return;
        }
        else
        {
            await next(request);
        }
    }
}