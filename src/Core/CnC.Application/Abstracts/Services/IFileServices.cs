using Microsoft.AspNetCore.Http;

namespace CnC.Application.Abstracts.Services;

public interface IFileServices
{
    Task<string> UploadAsync(IFormFile file, string folderName);
    public Task DeleteFileAsync(string relativePath);
}
