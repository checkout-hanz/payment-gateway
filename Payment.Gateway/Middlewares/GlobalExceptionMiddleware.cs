public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (FluentValidation.ValidationException e)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(
                e.Errors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync(
                $"Error executing the request.");
        }
    }
}