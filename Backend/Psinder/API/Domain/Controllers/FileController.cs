using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Psinder.API.Common.Filters;
using Psinder.API.Domain.Models;
using Psinder.API.Domain.Models.Files;

namespace Psinder.API.Domain.Controllers;

[ApiController]
[Route($"{ApplicationInfo.GroupName}/[controller]")]
public class FileController : ControllerBase
{
    private readonly IMediator _mediator;

    public FileController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [EnableCors("WebAppPolicy")]
    [HttpGet]
    [Route("{id}/unauthorized")]
    [ServiceFilter(typeof(RunAsTransactionFilter))]
    public async Task<IActionResult> GetFileUnauthorized([FromRoute] long id, CancellationToken cancellationToken)
    {
        var request = new GetFileRequest() { FileId = id };
        var fileDto = await _mediator.Send(new GetFileUnauthorizedMediatr(request), cancellationToken);

        return File(fileDto.Content, fileDto.ContentType, $"{fileDto.Name}{fileDto.ContentType}");
    }
}