using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using Psinder.API.Common.Exceptions;
using Psinder.API.Common.Models;

namespace Psinder.API.Common.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        context.Result = context.Exception switch
        {
            NotFoundException notFoundException => NotFoundResult(notFoundException),
            BadRequestException badRequestException => BadRequestResult(badRequestException),
            AccessDeniedException accessDeniedException => AccessDeniedResult(accessDeniedException),
            _ => UnknownErrorResult(context.Exception)
        };
    }

    private static IActionResult NotFoundResult(NotFoundException exception)
    {
        var payload = new ErrorDetails(NotFoundException.CODE, exception.Message);

        return new JsonResult(payload)
        {
            StatusCode = (int)HttpStatusCode.NotFound
        };
    }

    private static IActionResult BadRequestResult(BadRequestException exception)
    {
        var payload = new ErrorDetails(BadRequestException.CODE, exception.Message);

        return new JsonResult(payload)
        {
            StatusCode = (int)HttpStatusCode.BadRequest
        };
    }

    private static IActionResult AccessDeniedResult(AccessDeniedException exception)
    {
        var payload = new ErrorDetails(AccessDeniedException.CODE, exception.Message);

        return new JsonResult(payload)
        {
            StatusCode = (int)HttpStatusCode.Unauthorized
        };
    }

    private static IActionResult UnknownErrorResult(Exception exception)
    {
        var payload = new ErrorDetails("UNKNOWN", "An unknown error occurred");

        return new JsonResult(payload)
        {
            StatusCode = (int)HttpStatusCode.InternalServerError
        };
    }
}