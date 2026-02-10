using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ERP_API.Service
{
    public interface IBlobStorageService
    {
        Task<string> UploadAsync(IFormFile file);
        Task<(MemoryStream stream, string contentType)> DownloadAsync(string fileName);
        Task<bool> DeleteAsync(string fileName);
        string GetFileUrlAsync(string fileName);
        Task<string> GenerateViewUrlAsync(string blobName, TimeSpan expiration);


    }
}
