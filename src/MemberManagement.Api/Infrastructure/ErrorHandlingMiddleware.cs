using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace MemberManagement.Api.Infrastructure
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
                Trace.TraceInformation($"Request from {context.Connection.RemoteIpAddress} of " +
                                       $"{context.Request.Method} for {context.Request.Path}");
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (exception == null) return;

            var code = HttpStatusCode.InternalServerError;

            if (exception is NotFoundException)
            {
                code = HttpStatusCode.NotFound;
            }

            if (exception is ApplicationValidationException ||
                exception is DuplicateKeyException)
            {
                code = HttpStatusCode.BadRequest;
            }

            await WriteExceptionAsync(context, exception, code).ConfigureAwait(false);
        }

        private static async Task WriteExceptionAsync(HttpContext context, Exception exception, HttpStatusCode code)
        {
            //Log.Error(exception, string.Empty);
            Trace.TraceError(exception.Message);

            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)code;

            string message = exception.Message + (exception.InnerException == null
                ? string.Empty
                : " " + exception.InnerException.Message);

            await response.WriteAsync(JsonConvert.SerializeObject(new
            {
                error = new
                {
                    message,
                    exception = exception.GetType().Name
                }
            })).ConfigureAwait(false);
        }
    }

    public class NotFoundException : Exception
    {
        public NotFoundException() { }
        public NotFoundException(string message) : base(message) { }
    }

    public class ApplicationValidationException : Exception
    {
        public ApplicationValidationException() { }
        public ApplicationValidationException(string message) : base(message) { }
    }

    public class DuplicateKeyException : Exception
    {
        public DuplicateKeyException() { }
        public DuplicateKeyException(string message) : base(message) { }
    }
}
