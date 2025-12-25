using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using eCommerce.SharedLibrary.Exception;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.SharedLibrary.Middleware;

public class GlobalException(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // default exception
        string message = "Sorry an error occurred in the server, Kindly try again!";
        string title = "Error!!!";
        int statusCode = StatusCodes.Status500InternalServerError;

        try
        {
            await next(context);
            // too many requests 429
            if (context.Response.StatusCode == (int)HttpStatusCode.TooManyRequests)
            {
                title = "Warning!!!";
                message = "Sorry, the server is busy, please try again later.";
                statusCode = StatusCodes.Status429TooManyRequests;
                await ModifyHeader(context, title, message, statusCode);
                return;
            }
            // not found 404
            else if (context.Response.StatusCode == (int)HttpStatusCode.NotFound)
            {
                title = "Warning!!!";
                message = "Sorry, the page you are looking for is not found.";
                statusCode = StatusCodes.Status404NotFound;
                await ModifyHeader(context, title, message, statusCode);
                return;
            }
            // unauthorized 401
            else if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                title = "Warning!!!";
                message = "Sorry, you are not authorized to access this page.";
                statusCode = StatusCodes.Status401Unauthorized;
                await ModifyHeader(context, title, message, statusCode);
                return;
            }
            // bad request 400
            else if (context.Response.StatusCode == (int)HttpStatusCode.BadRequest)
            {
                title = "Warning!!!";
                message = "Sorry, the request is invalid.";
                statusCode = StatusCodes.Status400BadRequest;
                await ModifyHeader(context, title, message, statusCode);
                return;
            }
            // conflict 409
            else if (context.Response.StatusCode == (int)HttpStatusCode.Conflict)
            {
                title = "Warning!!!";
                message = "Sorry, the request is invalid.";
                statusCode = StatusCodes.Status409Conflict;
                await ModifyHeader(context, title, message, statusCode);
                return;
            }
            // forbidden 403
            else if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
            {
                title = "Alert !!!";
                message = "Sorry, you are not authorized to access this page.";
                statusCode = StatusCodes.Status403Forbidden;
                await ModifyHeader(context, title, message, statusCode);
                return;
            }
        }
        catch (System.Exception ex)
        {
            LogException.Log(ex);

            // If response started, we can't safely rewrite it.
            if (context.Response.HasStarted)
                throw;

            // Clear anything that might have been written before the exception
            context.Response.Clear();

            if (ex is TaskCanceledException or TimeoutException)
            {
                title = "Out of time";
                message = "Sorry, the request timed out.";
                statusCode = StatusCodes.Status408RequestTimeout;
            }

            await ModifyHeader(context, title, message, statusCode);
        }
    }

    private static async Task ModifyHeader(HttpContext context, string title, string message, int statusCode)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(new ProblemDetails
            {
                Title = title,
                Detail = message,
                Status = statusCode
            }),
            CancellationToken.None
        );
    }
}
