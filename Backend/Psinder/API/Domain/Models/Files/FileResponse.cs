using Microsoft.AspNetCore.Mvc;
using Psinder.API.Common.Models;

namespace Psinder.API.Domain.Models;

public class FileResponse
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string Extension { get; set; }

    public long ContentLength { get; set; }

    public string ContentType { get; set; }

    public Encodings Encoding { get; set; }

    public byte[] Content { get; set; }

    public static FileResponse FromDomain(DB.Common.Entities.File file, Encodings encoding = Encodings.UTF8)
    {
        return new FileResponse
        {
            Content = file.Content,
            ContentLength = file.ContentLength,
            ContentType = file.ContentType,
            Encoding = encoding,
            Extension = file.Extension,
            Id = file.Id,
            Name = file.Name
        };
    }
}
