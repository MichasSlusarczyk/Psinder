using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Psinder.API.Common.Filters;
using Psinder.API.Domain.Models.Shelters;
using Psinder.API.Domain.Models.Users;
using System.Net;

namespace Psinder.API.Domain.Controllers;

[ApiController]
[Route($"{ApplicationInfo.GroupName}/[controller]")]
public class ShelterController : ControllerBase
{
    private readonly IMediator _mediator;

    public ShelterController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [EnableCors("WebAppPolicy")]
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ServiceFilter(typeof(RunAsTransactionFilter))]
    [ProducesResponseType(typeof(AddShelterResponse), ((int)HttpStatusCode.OK))]
    public async Task<IActionResult> AddShelter([FromBody] AddShelterRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new AddShelterMediatr() { Request = request }, cancellationToken);

        return Ok(result);
    }

    [EnableCors("WebAppPolicy")]
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [Route("with-users")]
    [ServiceFilter(typeof(RunAsTransactionFilter))]
    [ProducesResponseType(typeof(AddShelterWithUsersResponse), ((int)HttpStatusCode.OK))]
    public async Task<IActionResult> AddShelterWithUsers([FromBody] AddShelterWithUsersRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new AddShelterWithUsersMediatr() { Request = request }, cancellationToken);

        return Ok(result);
    }

    [EnableCors("WebAppPolicy")]
    [HttpPut]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(AddShelterResponse), ((int)HttpStatusCode.OK))]
    public async Task<IActionResult> UpdateShelter([FromBody] UpdateShelterRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateShelterMediatr() { Request = request }, cancellationToken);

        return Ok(result);
    }

    [EnableCors("WebAppPolicy")]
    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(GetShelterResponse), ((int)HttpStatusCode.OK))]
    public async Task<IActionResult> GetShelter([FromRoute] long id, CancellationToken cancellationToken)
    {
        var request = new GetShelterRequest() { Id = id };
        var result = await _mediator.Send(new GetShelterMediatr() { Request = request }, cancellationToken);

        return Ok(result);
    }

    [EnableCors("WebAppPolicy")]
    [HttpGet]
    [Route("all")]
    [ProducesResponseType(typeof(GetSheltersResponse), ((int)HttpStatusCode.OK))]
    public async Task<IActionResult> GetShelters(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetSheltersMediatr(), cancellationToken);

        return Ok(result);
    }

    [EnableCors("WebAppPolicy")]
    [HttpGet]
    [Route("all/worker")]
    [Authorize(Roles = "Worker")]
    [ProducesResponseType(typeof(GetSheltersResponse), ((int)HttpStatusCode.OK))]
    public async Task<IActionResult> GetSheltersForWorker(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetSheltersForWorkerMediatr(), cancellationToken);

        return Ok(result);
    }
}