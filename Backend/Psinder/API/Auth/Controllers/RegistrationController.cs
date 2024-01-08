using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Psinder.API.Auth.Models;
using Psinder.API.Auth.Models.Registrations;
using Psinder.API.Common.Filters;
using System.Net;

namespace Psinder.API.Auth.Controllers;

[ApiController]
[Route($"{ApplicationInfo.GroupName}/[controller]")]
public class RegistrationController : ControllerBase
{
    private readonly IMediator _mediator;

    public RegistrationController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [EnableCors("WebAppPolicy")]
    [HttpPost]
    [Route($"Register")]
    [ServiceFilter(typeof(RunAsTransactionFilter))]
    [ProducesResponseType(typeof(AuthResponse), ((int)HttpStatusCode.OK))]
    public async Task<IActionResult> Register([FromBody] RegistrationRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new RegistrationMediatr() { Request = request }, cancellationToken);

        return Ok(result);
    }

    [EnableCors("WebAppPolicy")]
    [HttpPost]
    [Route($"Verify")]
    [ServiceFilter(typeof(RunAsTransactionFilter))]
    [ProducesResponseType(typeof(VerifyRegistrationResponse), ((int)HttpStatusCode.OK))]
    public async Task<IActionResult> VerifyRegistration([FromBody] VerifyRegistrationRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new VerifyRegistrationMediatr() { Request = request }, cancellationToken);
        return Ok(result);
    }
}