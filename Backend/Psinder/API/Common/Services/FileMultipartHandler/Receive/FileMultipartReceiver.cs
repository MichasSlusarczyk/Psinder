using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Psinder.API.Common.Models;
using System.Text.Json;

namespace Psinder.API.Common.Services.FileMultipartHandler;

public class FileMultipartReceiver<T> : IModelBinder where T : class
{
    private const string NAME_OBJECT_JSON = "object_json";
    private const string NAME_FILE_JSON = "file_json";
    private const string NAME_FILE_DATA = "file_data";
    private const string CONTENT_TYPE_JSON = "application/json";
    private const string CONTENT_TYPE_FILE = "application/octet-stream";

    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if(bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        string boundary = GetBoundary(bindingContext.HttpContext.Request.ContentType);
        var multipartContent = bindingContext.HttpContext.Request.Body;

        bindingContext.Result = ModelBindingResult.Success(await ProcessFileMultipart(multipartContent, boundary));
    }

    public async static Task<string> HandleMultipartResponseSerialized(HttpResponseMessage responseMessage)
    {
        var result = await HandleMultipartResponse(responseMessage);
        return JsonSerializer.Serialize(result);
    }

    public async static Task<FileMultipart<T>> HandleMultipartResponse(HttpResponseMessage responseMessage)
    {
        string boundary = GetBoundary(responseMessage.Content.Headers.ContentType.ToString());
        Stream multipartContent = await responseMessage.Content.ReadAsStreamAsync();

        return await ProcessFileMultipart(multipartContent, boundary);
    }

    private static async Task<FileMultipart<T>> ProcessFileMultipart(Stream multipartContent, string boundary)
    {
        var fileMultipart = new FileMultipart<T>();

        var multipartReader = new MultipartReader(boundary, multipartContent);

        multipartReader.BodyLengthLimit = long.MaxValue;

        MultipartSection section;

        while((section = await multipartReader.ReadNextSectionAsync()) != null)
        {
            var contentType = GetContentTypeName(section.ContentType);
            var contentDisposition = GetContentDispositionName(section.ContentDisposition);

            if(contentType == CONTENT_TYPE_JSON)
            {
                var serializedObject = await section.ReadAsStringAsync();

                if(contentDisposition == NAME_OBJECT_JSON)
                {
                    fileMultipart.FileMetadata = JsonSerializer.Deserialize<T>(serializedObject);
                }
                else if (contentDisposition.Contains(NAME_FILE_JSON))
                {
                    fileMultipart.Files.Add(
                        new FileMultipart<T>.FileData()
                        {
                            JsonContent = JsonSerializer.Deserialize<FileMultipart<T>.FileJson>(serializedObject)
                        });
                }
            }
            else if(contentType == CONTENT_TYPE_FILE && contentDisposition.Contains(NAME_FILE_DATA))
            {
                var file = fileMultipart.Files.ElementAtOrDefault((Index)GetFileIndex(contentDisposition));
                using var ms = new MemoryStream();
                section.Body.CopyToAsync(ms);
                file.DataContent = ms.ToArray();
            }
        }

        return fileMultipart;
    }

    private static string GetBoundary(string contentType)
    {
        string delimeter = "\"";
        int firstIndex = contentType.IndexOf(delimeter) + delimeter.Length;
        int lastIndex = contentType.LastIndexOf(delimeter);
        string boundary = contentType[firstIndex..lastIndex];

        return boundary;
    }

    public static string GetContentDispositionName(string contentDisposition)
    {
        string delimeter = "name=";
        int delimeterIndex = contentDisposition.IndexOf(delimeter) + delimeter.Length;
        string contentTypeName = contentDisposition.Substring(delimeterIndex);

        return contentTypeName;
    }

    private static string GetContentTypeName(string contentType)
    {
        int delimeterIndex = contentType.IndexOf(';');
        string contentTypeName = contentType[..delimeterIndex];

        return contentTypeName;
    }

    private static long GetFileIndex(string contentDisposition)
    {
        string delimeter = "_";
        int delimeterIndex = contentDisposition.LastIndexOf(delimeter) + delimeter.Length;
        long fileIndex = Convert.ToInt64(contentDisposition[delimeterIndex..]) - 1;

        return fileIndex;
    }
}

public class FileMultipartReceiver : FileMultipartReceiver<FilesMetadataNotUsed>
{
}
