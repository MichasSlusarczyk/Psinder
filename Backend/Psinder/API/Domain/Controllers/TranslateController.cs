using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Psinder.API.Domain.Models.Translate;
using System.Net;

namespace Psinder.API.Domain.Controllers;

[ApiController]
[Route($"{ApplicationInfo.GroupName}/[controller]")]
public class TranslateController : ControllerBase
{
    private readonly IMediator _mediator;

    public TranslateController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [EnableCors("WebAppPolicy")]
    [HttpPost]
    [ProducesResponseType(typeof(TranslateResponse), ((int)HttpStatusCode.OK))]
    public async Task<IActionResult> Translate([FromBody] TranslateRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new TranslateMediatr() { Request = request }, cancellationToken);

        return Ok(result);
    }
}