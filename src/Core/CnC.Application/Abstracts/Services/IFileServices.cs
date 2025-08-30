using Microsoft.AspNetCore.Http;

namespace CnC.Application.Abstracts.Services;

public interface IFileServices
{
    Task<string> UploadAsync(IFormFile file, string folderName = "product-files");
    Task DeleteFileAsync(string fileUrl);
    Task<bool> CheckFileExistsAsync(string fileUrl);
}