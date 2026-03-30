namespace WebApp.Web.Middleware;

using System.Net;


public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            if (!context.Response.HasStarted)
            {
                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "text/html";

                var message = _env.IsDevelopment()
                    ? $@"<h1>Error</h1><p>{ex.Message}</p><pre>{ex.StackTrace}</pre>"
                    : "<h2>Ocurrió un error inesperado</h2>";

                await context.Response.WriteAsync(message);
            }
        }
    }
}