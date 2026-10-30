using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MeetingRoomReservation.Api.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            if (exception is not ValidationException validationException)
                return false;

            var errors = validationException.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    group => group.Key,
                    group => group
                        .Select(e => e.ErrorMessage)
                        .ToArray());

            httpContext.Response.StatusCode =
                StatusCodes.Status400BadRequest;

            await httpContext.Response.WriteAsJsonAsync(
                new ValidationProblemDetails(errors)
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Validation failed."
                },
                cancellationToken);

            return true;
        }
    }
}
