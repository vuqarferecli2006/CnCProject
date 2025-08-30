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
        if (file?.Length == 0)
            throw new ArgumentException("File is null or empty", nameof(file));

        await using var stream = file.OpenReadStream();

        var isImage = IsImageFile(file.ContentType);

        UploadResult uploadResult;

        if (isImage)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folderName,
                UseFilename = true,
                UniqueFilename = true,
                Overwrite = false,
                Transformation = new Transformation().Quality("auto").FetchFormat("auto")
            };
            uploadResult = await _cloudinary.UploadAsync(uploadParams);
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
            uploadResult = await _cloudinary.UploadAsync(uploadParams);
        }

        if (uploadResult.StatusCode != HttpStatusCode.OK)
            throw new Exception($"Upload failed: {uploadResult.Error?.Message}");

        return uploadResult.SecureUrl.ToString();
    }

    public async Task DeleteFileAsync(string fileUrl)
    {
        if (string.IsNullOrWhiteSpace(fileUrl)) return;

        var publicId = ExtractPublicId(fileUrl);
        if (string.IsNullOrEmpty(publicId)) return;

        var isImage = IsImageFile(fileUrl);
        var result = await DeleteByResourceType(publicId, isImage);

        if (result.Result != "ok" && !isImage)
            await DeleteByResourceType(publicId, ResourceType.Auto);
    }

    public async Task<bool> CheckFileExistsAsync(string fileUrl)
    {
        var publicId = ExtractPublicId(fileUrl);
        if (string.IsNullOrEmpty(publicId)) return false;

        var resourceType = IsImageFile(fileUrl) ? ResourceType.Image : ResourceType.Raw;
        var result = await _cloudinary.GetResourceAsync(new GetResourceParams(publicId)
        {
            ResourceType = resourceType
        });

        return result.StatusCode == HttpStatusCode.OK;
    }

    #region Private Methods



    private async Task<DeletionResult> DeleteByResourceType(string publicId, bool isImage)
        => await DeleteByResourceType(publicId, isImage ? ResourceType.Image : ResourceType.Raw);


    private async Task<DeletionResult> DeleteByResourceType(string publicId, ResourceType resourceType)
        => await _cloudinary.DestroyAsync(new DeletionParams(publicId) { ResourceType = resourceType });


    private static bool IsImageFile(string input)
    {
        if (string.IsNullOrEmpty(input)) return false;

        if (input.StartsWith("image/", StringComparison.OrdinalIgnoreCase)) return true;

        var imageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp", ".tiff", ".svg" };
        var extension = Path.GetExtension(input).ToLowerInvariant();
        return imageExtensions.Contains(extension);
    }

    private static string ExtractPublicId(string url)
    {
        if (string.IsNullOrEmpty(url)) return string.Empty;

        var uri = new Uri(url);
        var segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
        var uploadIndex = Array.IndexOf(segments, "upload");

        if (uploadIndex == -1 || uploadIndex + 1 >= segments.Length)
            return string.Empty;

        var publicIdParts = segments.Skip(uploadIndex + 1).ToList();

        if (publicIdParts.Count > 0 && IsVersionSegment(publicIdParts[0]))
            publicIdParts.RemoveAt(0);

        if (publicIdParts.Count == 0) return string.Empty;
        
        if (IsImageFile(url))
        {
            var lastPart = publicIdParts.Last();
            var dotIndex = lastPart.LastIndexOf('.');
            if (dotIndex > 0)
                publicIdParts[publicIdParts.Count - 1] = lastPart.Substring(0, dotIndex);
        }

        return string.Join("/", publicIdParts);
    }

    private static bool IsVersionSegment(string segment)
        => segment.StartsWith("v") && segment.Length > 1 && long.TryParse(segment.Substring(1), out _);

    #endregion
}