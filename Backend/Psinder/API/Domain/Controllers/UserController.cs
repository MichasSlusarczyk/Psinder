using Psinder.API.Common.Filters;
using Psinder.API.Domain.Models.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Psinder.API.Domain.Controllers;

[ApiController]
[Route($"{ApplicationInfo.GroupName}/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [EnableCors("WebAppPolicy")]
    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(GetUserResponse), ((int)HttpStatusCode.OK))]
    public async Task<IActionResult> GetUser([FromRoute] long id, CancellationToken cancellationToken)
    {
        var request = new GetUserRequest() { Id = id };
        var result = await _mediator.Send(new GetUserMediatr() { Request = request }, cancellationToken);

        return Ok(result);
    }

    [EnableCors("WebAppPolicy")]
    [HttpGet]
    [Authorize(Roles = "Admin,Worker")]
    [Route("all")]
    [ProducesResponseType(typeof(GetUsersResponse), ((int)HttpStatusCode.OK))]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetUsersMediatr(), cancellationToken);

        return Ok(result);
    }

    [EnableCors("WebAppPolicy")]
    [HttpPut]
    [Authorize(Roles = "Admin,Worker,User")]
    [ProducesResponseType(typeof(UpdateUserResponse), ((int)HttpStatusCode.OK))]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateUserMediatr() { Request = request}, cancellationToken);

        return Ok(result);
    }

    [EnableCors("WebAppPolicy")]
    [HttpDelete]
    [Authorize(Roles = "Admin,Worker,User")]
    [Route("{id}/deactivate")]
    [ServiceFilter(typeof(RunAsTransactionFilter))]
    [ProducesResponseType(typeof(DeactivateUserResponse), ((int)HttpStatusCode.OK))]
    public async Task<IActionResult> DeactivateUser([FromRoute] long id, CancellationToken cancellationToken)
    {
        var request = new DeactivateUserRequest() { Id = id };
        var result = await _mediator.Send(new DeactivateUserMediatr() { Request = request }, cancellationToken);

        return Ok(result);
    }

    [EnableCors("WebAppPolicy")]
    [HttpDelete]
    [Authorize(Roles = "Admin")]
    [Route("{id}/block")]
    [ServiceFilter(typeof(RunAsTransactionFilter))]
    [ProducesResponseType(typeof(BlockUserResponse), ((int)HttpStatusCode.OK))]
    public async Task<IActionResult> BlockUser([FromRoute] long id, CancellationToken cancellationToken)
    {
        var request = new BlockUserRequest() { Id = id };
        var result = await _mediator.Send(new BlockUserMediatr() { Request = request }, cancellationToken);

        return Ok(result);
    }
}