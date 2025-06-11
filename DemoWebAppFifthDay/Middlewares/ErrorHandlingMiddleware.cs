namespace DemoWebAppFifthDay.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == StatusCodes.Status200OK)
            {
                _logger.LogInformation("Middleware - OK");
            }

            if (context.Response.StatusCode == StatusCodes.Status400BadRequest)
            {
                _logger.LogError("Middleware - Bad response");
            }

            await context.Response.WriteAsync("Can't parse that dude");
        }
    }
}