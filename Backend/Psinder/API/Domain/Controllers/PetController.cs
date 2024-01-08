using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Psinder.API.Common.Filters;
using Psinder.API.Domain.Models.Pets;
using Psinder.Db.Domain.Models.Pets;
using Psinder.DB.Common.Searching;
using System.Net;

namespace Psinder.API.Domain.Controllers;

[ApiController]
[Route($"{ApplicationInfo.GroupName}/[controller]")]
public class PetController : ControllerBase
{
    private readonly IMediator _mediator;

    public PetController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [EnableCors("WebAppPolicy")]
    [HttpPost]
    [Authorize(Roles = "Worker")]
    [ServiceFilter(typeof(RunAsTransactionFilter))]
    [ProducesResponseType(typeof(AddPetResponse), ((int)HttpStatusCode.Created))]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> AddPet([FromForm] AddPetRequest request, CancellationToken cancellationToken)
    {

        var result = await _mediator.Send(new AddPetMediatr(request), cancellationToken);

        return Ok(result);
    }

    [EnableCors("WebAppPolicy")]
    [HttpPut]
    [Authorize(Roles = "Worker")]
    [ServiceFilter(typeof(RunAsTransactionFilter))]
    [ProducesResponseType(typeof(AddPetResponse), ((int)HttpStatusCode.Created))]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdatePet([FromForm] UpdatePetRequest request, CancellationToken cancellationToken)
    {

        var result = await _mediator.Send(new UpdatePetMediatr(request), cancellationToken);

        return Ok(result);
    }

    [EnableCors("WebAppPolicy")]
    [HttpGet("{id}")]
    [ServiceFilter(typeof(RunAsTransactionFilter))]
    [ProducesResponseType(typeof(GetPetResponse), ((int)HttpStatusCode.OK))]
    public async Task<IActionResult> GetPet(long id, CancellationToken cancellationToken)
    {
        var request = new GetPetRequest() { PetId = id };
        var result = await _mediator.Send(new GetPetMediatr(request), cancellationToken);

        return Ok(result);
    }

    [EnableCors("WebAppPolicy")]
    [HttpDelete("{id}")]
    [Authorize(Roles = "Worker")]
    [ServiceFilter(typeof(RunAsTransactionFilter))]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeletePet(long id, CancellationToken cancellationToken)
    {
        var requestModel = new DeletePetRequest(id);
        var result = await _mediator.Send(new DeletePetMediatr(requestModel), cancellationToken);

        return Ok();
    }

    [EnableCors("WebAppPolicy")]
    [HttpPost("list")]
    [ProducesResponseType(typeof(GetPetsListResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GePetstList(
        [FromBody] GetPetsListFilters filters,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        [FromQuery] string? orderBy,
        CancellationToken cancellationToken)
    {
        var request = new GetPetsListMediatr(
            filters,
            PageInfo.FromQuery(page, pageSize) ?? PageInfo.Default,
            SortInfo<PetsListSortColumns>.Parse(orderBy));
        var result = await _mediator.Send(request, cancellationToken);

        return Ok(result);
    }
}