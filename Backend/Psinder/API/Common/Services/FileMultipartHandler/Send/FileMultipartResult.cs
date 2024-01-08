using Microsoft.AspNetCore.Mvc;
using Psinder.API.Common.Models;

namespace Psinder.API.Common.Services.FileMultipartHandler;

public class FileMultipartResult<T> : IActionResult where T : class
{
    public FileMultipartResult(FileMultipart<T> request)
    {
        Content = new FileMultipartFactory<T>().CreateContent(request);
    }

    public MultipartContent Content { get; private set; }

    public Task ExecuteResultAsync(ActionContext context)
    {
        context.HttpContext.Response.ContentType = Content.Headers.ContentType.ToString();
        context.HttpContext.Response.Headers.ContentDisposition = Content.Headers.ContentDisposition.ToString();

        return Content.CopyToAsync(context.HttpContext.Response.Body);
    }
}

public class FileMultipartResult : FileMultipartResult<FilesMetadataNotUsed> 
{ 
    public FileMultipartResult(FileMultipart<FilesMetadataNotUsed> request) : base(request) 
    { 
    }
}