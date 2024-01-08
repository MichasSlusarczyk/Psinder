using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Psinder.API.Auth.Models;
using Psinder.API.Auth.Models.Logins;
using Psinder.API.Common.Filters;
using System.Net;

namespace Psinder.API.Auth.Controllers;

[ApiController]
[Route($"{ApplicationInfo.GroupName}/[controller]")]
public class LoginController : ControllerBase
{
    private readonly IMediator _mediator;

    public LoginController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [EnableCors("WebAppPolicy")]
    [HttpPost]
    [ServiceFilter(typeof(RunAsTransactionFilter))]
    [ProducesResponseType(typeof(AuthResponse), ((int)HttpStatusCode.OK))]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new LoginMediatr() { Request = request }, cancellationToken);

        return Ok(result);
    }

    [EnableCors("WebAppPolicy")]
    [HttpPost]
    [Route($"google")]
    [ServiceFilter(typeof(RunAsTransactionFilter))]
    [ProducesResponseType(typeof(AuthResponse), ((int)HttpStatusCode.OK))]
    public async Task<IActionResult> LoginWithGoogle([FromBody] LoginGoogleRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new LoginGoogleMediatr() { Request = request }, cancellationToken);

        return Ok(result);
    }

    [EnableCors("WebAppPolicy")]
    [HttpPost]
    [Route($"facebook")]
    [ServiceFilter(typeof(RunAsTransactionFilter))]
    [ProducesResponseType(typeof(AuthResponse), ((int)HttpStatusCode.OK))]
    public async Task<IActionResult> LoginWithFacebook([FromBody] LoginFacebookRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new LoginFacebookMediatr() { Request = request }, cancellationToken);

        return Ok(result);
    }

    [EnableCors("WebAppPolicy")]
    [HttpPost]
    [Route($"refresh")]
    [ServiceFilter(typeof(RunAsTransactionFilter))]
    [ProducesResponseType(typeof(RefreshTokenResponse), ((int)HttpStatusCode.OK))]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new RefreshTokenMediatr() { Request = request }, cancellationToken);

        return Ok(result);
    }

    [EnableCors("WebAppPolicy")]
    [HttpPost]
    [Authorize(Roles = "Admin,Worker,User")]
    [Route($"change-password")]
    [ServiceFilter(typeof(RunAsTransactionFilter))]
    [ProducesResponseType(typeof(ChangePasswordResponse), ((int)HttpStatusCode.OK))]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ChangePasswordMediatr() { Request = request }, cancellationToken);

        return Ok(result);
    }

    [EnableCors("WebAppPolicy")]
    [HttpPost]
    [Route($"send-password-reminder")]
    [ServiceFilter(typeof(RunAsTransactionFilter))]
    [ProducesResponseType(typeof(SendPasswordReminderResponse), ((int)HttpStatusCode.OK))]
    public async Task<IActionResult> SendPasswordReminder([FromBody] SendPasswordReminderRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new SendPasswordReminderMediatr() { Request = request }, cancellationToken);

        return Ok(result);
    }

    [EnableCors("WebAppPolicy")]
    [HttpPost]
    [Route($"remind-password")]
    [ServiceFilter(typeof(RunAsTransactionFilter))]
    [ProducesResponseType(typeof(RemindPasswordResponse), ((int)HttpStatusCode.OK))]
    public async Task<IActionResult> RemindPassword([FromBody] RemindPasswordRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new RemindPasswordMediatr() { Request = request }, cancellationToken);

        return Ok(result);
    }

    [EnableCors("WebAppPolicy")]
    [HttpPost]
    [Route($"reset-password")]
    [ServiceFilter(typeof(RunAsTransactionFilter))]
    [ProducesResponseType(typeof(ResetPasswordResponse), ((int)HttpStatusCode.OK))]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ResetPasswordMediatr() { Request = request }, cancellationToken);

        return Ok(result);
    }
}