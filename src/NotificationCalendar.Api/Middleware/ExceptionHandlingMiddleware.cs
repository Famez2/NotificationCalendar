using FluentValidation;
using NotificationCalendar.Api.Contracts;
using NotificationCalendar.Common.Exceptions;
using System.Diagnostics;
using System.Text.Json;

namespace NotificationCalendar.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    private const string DefaultErrorMessage = "Внутренняя ошибка сервера";
    private const int DefaultErrorStatusCode = StatusCodes.Status500InternalServerError;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            await _next(context);
        }
        catch (BadRequestException ex)
        {
            stopwatch.Stop();

            if (ex.Errors.Count != 0)
            {
                await HandleExceptionAsync(context, "Ошибка валидации", StatusCodes.Status400BadRequest, ex.Errors);
            }
            else
            {
                await HandleExceptionAsync(context, ex.Message, StatusCodes.Status400BadRequest);
            }

            logger.LogError(ex,
                "Request processing has been failed with bad request. {Message}. Url: {RequestPath}. HttpMethod: {HttpMethod}. StatusCode: {StatusCode}. Duration: {Duration}",
                ex.Message, context.Request.Path, context.Request.Method,
                context.Response.StatusCode, stopwatch.ElapsedMilliseconds);
        }
        catch (NotFoundException ex)
        {
            stopwatch.Stop();

            if (ex.Errors.Count != 0)
            {
                await HandleExceptionAsync(context, "Ресурс не найден", StatusCodes.Status404NotFound, ex.Errors);
            }
            else
            {
                await HandleExceptionAsync(context, ex.Message, StatusCodes.Status404NotFound);
            }

            logger.LogError(ex,
                "Requested resource was not found. {Message}. Url: {RequestPath}. HttpMethod: {HttpMethod}. StatusCode: {StatusCode}. Duration: {Duration}",
                ex.Message, context.Request.Path, context.Request.Method,
                context.Response.StatusCode, stopwatch.ElapsedMilliseconds);
        }        catch (OperationCanceledException ex)
        {
            stopwatch.Stop();

            await HandleExceptionAsync(context, "Запрос был отменен", StatusCodes.Status499ClientClosedRequest);

            logger.LogError(ex,
                "Request processing has been cancelled by client. Url: {RequestPath}. HttpMethod: {RequestMethod}. StatusCode: {StatusCode}",
                context.Request.Path, context.Request.Method,
                context.Response.StatusCode);
        }
        catch (ValidationException ex)
        {
            stopwatch.Stop();

            var errors = ex.Errors
                .GroupBy(x => x.PropertyName, x => x.ErrorMessage, (propertyName, errorMessages) => new
                {
                    Key = propertyName,
                    Values = errorMessages.Distinct().ToArray()
                })
                .ToDictionary(k => k.Key, v => v.Values);

            await HandleExceptionAsync(context, "Ошибка валидации", StatusCodes.Status400BadRequest, errors);

            logger.LogError(ex,
                "Validation error occurred. Url: {RequestPath}. HttpMethod: {RequestMethod}. StatusCode: {StatusCode}",
                context.Request.Path, context.Request.Method, context.Response.StatusCode);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            await HandleExceptionAsync(context, DefaultErrorMessage, DefaultErrorStatusCode);

            logger.LogError(ex,
                "Request processing has been failed. {Message}. Url: {RequestPath}. HttpMethod: {HttpMethod}. StatusCode: {StatusCode}. Duration: {Duration}",
                ex.Message, context.Request.Path, context.Request.Method,
                context.Response.StatusCode, stopwatch.ElapsedMilliseconds);
        }
    }

    private async Task HandleExceptionAsync(
        HttpContext context,
        string errorMessage,
        int statusCode,
        Dictionary<string, string[]>? errors = default)
    {
        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsJsonAsync(new ApiResponseV1
        {
            Error = new ApiResponseV1.ResponseErrorModel
            {
                Message = errorMessage ?? DefaultErrorMessage,
                StatusCode = statusCode,
                Errors = errors
            }
        }, new JsonSerializerOptions
        {
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }
}
