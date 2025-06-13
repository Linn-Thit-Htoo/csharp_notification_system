namespace notification_system.notification.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        var result = BaseResponse<object>.Fail(exception);
        httpContext.Response.ContentType = "application/json";

        _logger.LogError("Gloobal Exception Handler: {0}", exception.ToString());

        string jsonStr = JsonConvert.SerializeObject(result);
        await httpContext.Response.WriteAsync(jsonStr, cancellationToken);

        return true;
    }
}
