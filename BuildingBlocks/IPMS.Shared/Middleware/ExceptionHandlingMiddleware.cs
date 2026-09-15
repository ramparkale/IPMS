//using System.Net;
//using System.Text.Json;
//using IPMS.Shared.DTOs;
//using IPMS.Shared.Exceptions;

//namespace IPMS.Shared.Middleware;

//public class ExceptionHandlingMiddleware
//{
//    private readonly RequestDelegate _next;
//    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

//    public ExceptionHandlingMiddleware( 
//        RequestDelegate next,
//        ILogger<ExceptionHandlingMiddleware> logger)
//    {
//        _next = next;
//        _logger = logger;
//    }

//    public async Task InvokeAsync(HttpContext context)
//    {
//        try
//        {
//            await _next(context);
//        }
//        catch (BusinessException ex)
//        {
//            await HandleExceptionAsync(
//                context,
//                ex,
//                HttpStatusCode.BadRequest);
//        }
//        catch (NotFoundException ex)
//        {
//            await HandleExceptionAsync(
//                context,
//                ex,
//                HttpStatusCode.NotFound);
//        }
//        catch (UnauthorizedException ex)
//        {
//            await HandleExceptionAsync(
//                context,
//                ex,
//                HttpStatusCode.Unauthorized);
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(
//                ex,
//                "Unhandled exception occurred.");

//            await HandleExceptionAsync(
//                context,
//                ex,
//                HttpStatusCode.InternalServerError);
//        }
//    }

//    private static async Task HandleExceptionAsync(
//        HttpContext context,
//        Exception exception,
//        HttpStatusCode statusCode)
//    {
//        context.Response.ContentType = "application/json";
//        context.Response.StatusCode = (int)statusCode;

//        var correlationId =
//            context.Items["X-Correlation-ID"]?.ToString();

//        var response = new ErrorResponse
//        {
//            Success = false,
//            Message = exception.Message,
//            CorrelationId = correlationId,
//            Timestamp = DateTime.UtcNow
//        };

//        var json = JsonSerializer.Serialize(response);

//        await context.Response.WriteAsync(json);
//    }
//}