using Microsoft.AspNetCore.Http;
using Serilog;
using System.Diagnostics;

namespace PhoneAddressBook.API.Middleware;

public class SerilogLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public SerilogLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
        _logger = Log.ForContext<SerilogLoggingMiddleware>();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        _logger.Information("Handling request: {Method} {Path}", context.Request.Method, context.Request.Path);

        var originalBodyStream = context.Response.Body;

        try
        {
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context);

                stopwatch.Stop();

                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                _logger.Information("Handled request: {Method} {Path} responded {StatusCode} in {ElapsedMilliseconds} ms",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode,
                    stopwatch.Elapsed.TotalMilliseconds);


                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.Error(ex, "An exception occurred while handling the request: {Method} {Path}", context.Request.Method, context.Request.Path);
            throw;
        }
        finally
        {
            context.Response.Body = originalBodyStream;
        }
    }
}