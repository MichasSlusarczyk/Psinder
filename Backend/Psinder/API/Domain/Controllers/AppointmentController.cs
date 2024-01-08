using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Psinder.API.Common.Filters;
using Psinder.API.Domain.Models.Appointments;
using System.Net;

namespace Psinder.API.Domain.Controllers;

[ApiController]
[Route($"{ApplicationInfo.GroupName}/[controller]")]
public class AppointmentController : ControllerBase
{
    private readonly IMediator _mediator;

    public AppointmentController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [EnableCors("WebAppPolicy")]
    [HttpGet]
    [Authorize(Roles = "Admin,Worker")]
    [Route("shelter/{id}")]
    [ServiceFilter(typeof(RunAsTransactionFilter))]
    [ProducesResponseType(typeof(GetAllAppointmentsForShelterResponse), ((int)HttpStatusCode.OK))]
    public async Task<IActionResult> GetAllAppointmentsForShelter([FromRoute] long id, CancellationToken cancellationToken)
    {
        var request = new GetAllAppointmentsForShelterRequest() { ShelterId = id };
        var result = await _mediator.Send(new GetAllAppointmentsForShelterMediatr() { Request = request }, cancellationToken);

        return Ok(result);
    }


    [EnableCors("WebAppPolicy")]
    [HttpGet]
    [Authorize(Roles = "Admin,Worker,User")]
    [Route("pet/{id}")]
    [ServiceFilter(typeof(RunAsTransactionFilter))]
    [ProducesResponseType(typeof(GetAllAppointmentsForPetResponse), ((int)HttpStatusCode.OK))]
    public async Task<IActionResult> GetAllAppointmentsForPet([FromRoute] long id, CancellationToken cancellationToken)
    {
        var request = new GetAllAppointmentsForPetRequest() { PetId = id };
        var result = await _mediator.Send(new GetAllAppointmentsForPetMediatr() { Request = request }, cancellationToken);

        return Ok(result);
    }

    [EnableCors("WebAppPolicy")]
    [HttpGet]
    [Authorize(Roles = "Admin,Worker,User")]
    [Route("user/{id}")]
    [ServiceFilter(typeof(RunAsTransactionFilter))]
    [ProducesResponseType(typeof(GetAllAppointmentsForUserResponse), ((int)HttpStatusCode.OK))]
    public async Task<IActionResult> GetAllAppointmentsForUser([FromRoute] long id, CancellationToken cancellationToken)
    {
        var request = new GetAllAppointmentsForUserRequest() { UserId = id };
        var result = await _mediator.Send(new GetAllAppointmentsForUserMediatr() { Request = request }, cancellationToken);

        return Ok(result);
    }

    [EnableCors("WebAppPolicy")]
    [HttpDelete]
    [Authorize(Roles = "Admin,Worker,User")]
    [Route("{id}/cancel")]
    [ServiceFilter(typeof(RunAsTransactionFilter))]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> CancelAppointment([FromRoute] long id, CancellationToken cancellationToken)
    {
        var request = new CancelAppointmentRequest() { Id = id };
        await _mediator.Send(new CancelAppointmentMediatr() { Request = request }, cancellationToken);

        return Ok();
    }

    [EnableCors("WebAppPolicy")]
    [HttpPost]
    [Authorize(Roles = "Admin,Worker,User")]
    [ServiceFilter(typeof(RunAsTransactionFilter))]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> AddAppointment([FromBody] AddAppointmentRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new AddAppointmentMediatr() { Request = request }, cancellationToken);

        return Ok();
    }
}