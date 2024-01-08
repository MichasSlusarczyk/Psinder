using Psinder.API.Domain.Models;

namespace Psinder.API.Common.Models;

[Serializable]
public class FileMultipart<T> where T : class
{
    public T? FileMetadata { get; set; }
    public List<FileData> Files { get; set; } = new List<FileData>();

    public class FileData
    {
        public FileJson JsonContent { get; set; }
        public byte[] DataContent { get; set; }
    }

    public class FileJson
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public string ContentType { get; set; }
        public long ContentLength { get; set; }
        public Encodings Encoding { get; set; }
    }

    public static FileMultipart<T> New(List<FileResponse> files, T? jsonObject = null)
    {
        var result = new FileMultipart<T>();
        foreach(var file in files)
        {
            result.Files.Add(new FileMultipart<T>.FileData()
            {
                JsonContent = new FileMultipart<T>.FileJson
                {
                    Id = file.Id,
                    Name = file.Name,
                    ContentLength = file.ContentLength,
                    ContentType = file.ContentType,
                    Extension = file.Extension,
                    Encoding = file.Encoding
                },
                DataContent = file.Content
            });
        };

        result.FileMetadata = jsonObject;

        return result;
    }

    public static FileMultipart<T> New(FileResponse file, T? jsonObject = null)
    {
        var result = new FileMultipart<T>();

        result.Files.Add(new FileMultipart<T>.FileData()
        {
            JsonContent = new FileMultipart<T>.FileJson
            {
                Id = file.Id,
                Name = file.Name,
                ContentLength = file.ContentLength,
                ContentType = file.ContentType,
                Extension = file.Extension,
                Encoding = file.Encoding
            },
            DataContent = file.Content
        });

        result.FileMetadata = jsonObject;

        return result;
    }

    public static FileResponse ToFileDto(FileMultipart<T> fileMultipart, int fileIndex = 1)
    {
        if(fileMultipart == null) throw new ArgumentNullException(nameof(fileMultipart));

        var file = fileMultipart.Files.ElementAt(fileIndex - 1);

        return new FileResponse()
        {
            Content = file.DataContent,
            ContentLength = file.JsonContent.ContentLength,
            ContentType = file.JsonContent.ContentType,
            Encoding = file.JsonContent.Encoding,
            Extension = file.JsonContent.Extension,
            Id = file.JsonContent.Id,
            Name = file.JsonContent.Name
        };
    }
}

[Serializable]
public class FileMultipart : FileMultipart<FilesMetadataNotUsed>
{
}

public class FilesMetadataNotUsed { }