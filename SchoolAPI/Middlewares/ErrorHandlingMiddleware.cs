using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
namespace SchoolAPI.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exeption occurred while processing the request.");
            var status = ex switch
            {
                ArgumentException => 400,
                ValidationException => 400,
                KeyNotFoundException => 404,
                InvalidOperationException => 409,
                UnauthorizedAccessException => 401,
                _ => 500
            };
            var problem = new ProblemDetails
            {
                Status = status,
                Title = status == 500 ? "Unexpected error occurred." : ex?.Message,
                Type = status == 500 ? $"https://example.com/problems/{status}" : "about:blank",
                Detail = status == 500 ? null : ex?.Message,
                Instance = context.Request.Path
            };

            context.Response.StatusCode = status;
            context.Response.ContentType = "application/problem+json";
            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}
