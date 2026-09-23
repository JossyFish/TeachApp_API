using Auth.Domain.Models.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {

        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            var problemDetails = exception switch
            {
                UserAlreadyExistException userExists => new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                    Title = "Conflict",
                    Status = StatusCodes.Status409Conflict,
                    Detail = exception.Message,
                    Extensions = { ["email"] = ((UserAlreadyExistException)exception).Email }
                },
                UserNotFoundException userNotFound => new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                    Title = "Not Found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = exception.Message,
                    Extensions = { ["email"] = userNotFound.Email, ["userId"] = userNotFound.UserId }
                },

                //UserUnauthorizedException unauthorized => new ProblemDetails
                //{
                //    Type = "https://tools.ietf.org/html/rfc7235#section-3.1",
                //    Title = "Unauthorized",
                //    Status = StatusCodes.Status401Unauthorized,
                //    Detail = exception.Message
                //},

                ConfirmCodeExpiredException codeExpired => new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3",
                    Title = "Code Expired",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = exception.Message,
                    Extensions = { ["confirmCode"] = codeExpired.ConfirmCode }
                },

                InvalidConfirmCodeException invalidCode => new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "Invalid Code",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = exception.Message,
                    Extensions = { ["confirmCode"] = invalidCode.ConfirmCode }
                },

                InvalidGoogleTokenException invalidGoogle => new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7235#section-3.1",
                    Title = "Invalid Google Token",
                    Status = StatusCodes.Status401Unauthorized,
                    Detail = invalidGoogle.Message
                },

                EmailNotVerifiedException emailNotVerified => new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "Email Not Verified",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = emailNotVerified.Message
                },

                RoleNotAllowedException roleNotAllowed => new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3",
                    Title = "Role Not Allowed",
                    Status = StatusCodes.Status403Forbidden,
                    Detail = roleNotAllowed.Message
                },

                RoleMismatchException roleMismatch => new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3",
                    Title = "Role Mismatch",
                    Status = StatusCodes.Status403Forbidden,
                    Detail = roleMismatch.Message
                },

                InvalidCredentialsException invalidCredentials => new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "Invalid Credentials",
                    Status = StatusCodes.Status401Unauthorized,
                    Detail = exception.Message,
                    Extensions = {  }
                },

                Domain.Models.Exceptions.ValidationException validation => new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "Validation error",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = exception.Message,
                    Extensions = { ["errors"] = ((Domain.Models.Exceptions.ValidationException)exception).Errors }
                },

                _ => new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                    Title = "Server error",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = "Server error has occurred"
                }
            };

            context.Response.StatusCode = problemDetails.Status ?? 500;
            await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }

    }
}
