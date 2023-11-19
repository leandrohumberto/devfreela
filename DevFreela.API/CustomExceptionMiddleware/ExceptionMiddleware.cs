using DevFreela.API.Models;
using DevFreela.Core.Exceptions;
using DevFreela.Core.Services;
using System.Net;

namespace DevFreela.API.CustomExceptionMiddleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerService _logger;

        public ExceptionMiddleware(RequestDelegate next, ILoggerService logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (LoginFailException ex)
            {
                _logger.LogError($"Login failed for credentials {ex.Email} - {ex.Password}: {ex}");
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (InvalidProjectException ex)
            {
                _logger.LogError($"Invalid project - {ex.IdProject}: {ex}");
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (InvalidUserException ex)
            {
                _logger.LogError($"Invalid user - {ex.IdUser}: {ex}");
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (InvalidUserEmailException ex)
            {
                _logger.LogError($"Invalid user email - {ex.Email}: {ex}");
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(context, HttpStatusCode.InternalServerError,
                    "Internal Server Error.");
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsync(new ErrorDetails(context.Response.StatusCode,
                message).ToString());
        }
    }
}
