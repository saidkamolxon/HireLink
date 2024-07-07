using HireLink.Api.Errors;

namespace HireLink.Api.Middleware;

public class ExceptionMiddleware(RequestDelegate request, ILogger<ExceptionMiddleware> logger)
{
    public readonly RequestDelegate request = request;
    public readonly ILogger<ExceptionMiddleware> logger = logger;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await this.request(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            this.logger.LogError(ex, ex.Message, ex.StackTrace);
            await context.Response.WriteAsJsonAsync(new ApiException
            (
                statusCode: context.Response.StatusCode,
                message: ex.Message,
                details: ex.StackTrace
            ));
        }
    }
}
