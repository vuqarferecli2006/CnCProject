using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CnC.Application.Abstracts.Services;
using CnC.Application.Shared.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net;

namespace CnC.Infrastructure.Services;

public class CloudinaryUploadService : IFileServices
{
    private readonly Cloudinary _cloudinary;
    public CloudinaryUploadService(IOptions<CloudinarySettings> config)
    {
        var account = new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret
        );  

        _cloudinary = new Cloudinary(account);
    }

    public async Task<string> UploadAsync(IFormFile file, string folderName = "product-files")
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File is null or empty", nameof(file));

        await using var stream = file.OpenReadStream();

        // Faylın MIME tipinə baxaq
        var isImage = file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase);

        if (isImage)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folderName,
                UseFilename = true,
                UniqueFilename = true,
                Overwrite = false,
                Transformation = new Transformation()
                    .Quality("auto")
                    .FetchFormat("auto")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode != HttpStatusCode.OK)
                throw new Exception("Failed to upload image to Cloudinary: " + uploadResult.Error?.Message);

            return uploadResult.SecureUrl.ToString();
        }
        else
        {
            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folderName,
                UseFilename = true,
                UniqueFilename = true,
                Overwrite = false
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode != HttpStatusCode.OK)
                throw new Exception("Failed to upload file to Cloudinary: " + uploadResult.Error?.Message);

            return uploadResult.SecureUrl.ToString();
        }
    }

    public async Task DeleteFileAsync(string fileUrl)
    {
        var publicId = GetFileIdFromUrl(fileUrl);
        if (string.IsNullOrEmpty(publicId))
            return;

        var deletionParams = new DeletionParams(publicId);

        await _cloudinary.DestroyAsync(deletionParams);
    }

    private string GetFileIdFromUrl(string url)
    {

        try
        {
            var uri = new Uri(url);
            var segments = uri.AbsolutePath.Split('/');
            var fileName = Path.GetFileNameWithoutExtension(segments.Last());
            var folder = segments.Length >= 3 ? segments[^2] : null;

            return string.IsNullOrEmpty(folder) 
                ? fileName 
                : $"{folder}/{fileName}";
        }
        catch 
        {
            return string.Empty;
        }

    }
}
