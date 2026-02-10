using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ERP.Models
{
    public static class ErrorHandlerExtensions
    {
        public static IApplicationBuilder UseErrorHandler(
                                          this IApplicationBuilder appBuilder,
                                          ILoggerFactory loggerFactory)
        {
            return appBuilder.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    var exceptionHandlerFeature = context
                                                    .Features
                                                    .Get<IExceptionHandlerFeature>();

                    if (exceptionHandlerFeature != null)
                    {

                        var logger = loggerFactory.CreateLogger("ErrorHandler");
                        logger.LogError($"Error: {exceptionHandlerFeature.Error}");

                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "application/json";

                        var json = new
                        {
                            context.Response.StatusCode,
                            Message = "Internal Server Error",
                        };

                        await context.Response.WriteAsync(JsonConvert.SerializeObject(json));
                    }
                });
            });
        }
    }
}

public class ErrorHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger _log;

    public ErrorHandler(RequestDelegate next, ILoggerFactory log)
    {
        this._next = next;
        this._log = log.CreateLogger("MyErrorHandler");
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
       catch (DbUpdateException ex)
        {
            await HandleErrorAsync(httpContext, ex);
        }
    }

    private async Task HandleErrorAsync(HttpContext context, Exception exception)
    {
        var errorResponse = new ErrorResponse();

        errorResponse.StatusCode = HttpStatusCode.InternalServerError;
        errorResponse.Message = exception.Message;

        _log.LogError($"Error: {exception.Message}");
        _log.LogError($"Stack: {exception.StackTrace}");

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)errorResponse.StatusCode;
        await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
    }
}

public class ErrorResponse
{
    public string Message { get; set; }
    public HttpStatusCode StatusCode { get; set; }
}

public static class ErrorHandlerExtensions
{
    public static IApplicationBuilder UseMyErrorHandler(this IApplicationBuilder appBuilder)
    {
        return appBuilder.UseMiddleware<ErrorHandler>();
    }
}


