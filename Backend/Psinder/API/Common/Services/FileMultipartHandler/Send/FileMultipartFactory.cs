using Psinder.API.Common.Helpers;
using Psinder.API.Common.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Psinder.API.Common.Services.FileMultipartHandler;

public class FileMultipartFactory<T> : IDisposable where T : class
{
    private const string NAME_OBJECT_JSON = "object_json";
    private const string NAME_FILE_JSON = "file_json";
    private const string NAME_FILE_DATA = "file_data";
    private const string NAME_MULTIPART = "file_multipart";
    private const string DISPOSITION_MULTIPART = "mixed";
    private const string CONTENT_TYPE_JSON = "application/json";
    private const string CONTENT_TYPE_FILE = "application/octet-stream";

    private readonly Encodings jsonEncoding = Encodings.UTF8;
    private bool disposed = false;

    private MultipartContent Content { get; set; }

    public FileMultipartFactory()
    {
        InitializeContent();
    }

    private void InitializeContent()
    {
        Content = new MultipartContent(DISPOSITION_MULTIPART);
        Content.Headers.ContentDisposition = new ContentDispositionHeaderValue(DISPOSITION_MULTIPART)
        {
            Name = NAME_MULTIPART
        };
    }

    public MultipartContent CreateContent(FileMultipart<T> request)
    {
        AddJson<T>(request.FileMetadata, NAME_OBJECT_JSON);
        foreach(var file in request.Files.Select((value, idx) => new {value, idx }))
        {
            var fileIndex = file.idx + 1;
            var fileJsonName = string.Concat(NAME_FILE_JSON, '_', fileIndex.ToString());
            var fileDataName = string.Concat(NAME_FILE_DATA, '_', fileIndex.ToString());

            var fileDto = FileMultipart<T>.ToFileDto(request);

            AddJson<FileMultipart<T>.FileJson>(file.value.JsonContent, fileJsonName);
            AddFile(file.value.DataContent, fileDto.Encoding, fileDataName);
        }

        return Content;
    }

    private void AddJson<T>(T? jsonObject, string jsonName)
    {
        if (jsonObject != null)
        {
            var encoding = EncodingHelper.GetEncoding(jsonEncoding);
            string json = JsonSerializer.Serialize(jsonObject);
            StringContent content = new StringContent(json, encoding);
            content.Headers.ContentType = new MediaTypeHeaderValue(CONTENT_TYPE_JSON)
            {
                CharSet = encoding.WebName
            };
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue(DISPOSITION_MULTIPART)
            {
                Name = jsonName
            };

            Content.Add(content);
        }
    }

    private void AddFile(byte[] fileContent, Encodings fileEncoding, string dataName)
    {
        if(fileContent != null)
        {
            var encoding = EncodingHelper.GetEncoding(fileEncoding);
            ByteArrayContent content = new ByteArrayContent(fileContent);
            content.Headers.ContentType = new MediaTypeHeaderValue(CONTENT_TYPE_FILE)
            {
                CharSet = encoding.WebName
            };
            content.Headers.Add("Content-Transfer-Encoding", "binary");
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue(DISPOSITION_MULTIPART)
            {
                Name = dataName
            };

            Content.Add(content);
        }
    }

    public void Dispose()
    {

    }

    public virtual void Dispose(bool disposing)
    {
        if (disposed)
            return;

        if (disposing)
        {
            if (Content != null)
            {
                Content.Dispose();
            }
        }

        disposed = true;
    }
}

public class FileMultipartFactory : FileMultipartFactory<FilesMetadataNotUsed>
{

}