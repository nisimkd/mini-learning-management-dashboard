using LearningManagementDashboard.Api.Constants;

namespace LearningManagementDashboard.Api.Middleware;

/// <summary>
/// Middleware to add security headers to all HTTP responses
/// </summary>
public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public SecurityHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Add security headers
        context.Response.Headers.TryAdd(ApiConstants.Headers.XContentTypeOptions, ApiConstants.HeaderValues.NoSniff);
        context.Response.Headers.TryAdd(ApiConstants.Headers.XFrameOptions, ApiConstants.HeaderValues.Deny);
        context.Response.Headers.TryAdd(ApiConstants.Headers.XXssProtection, ApiConstants.HeaderValues.XssBlock);
        context.Response.Headers.TryAdd(ApiConstants.Headers.ReferrerPolicy, ApiConstants.HeaderValues.StrictOriginWhenCrossOrigin);

        await _next(context);
    }
}
