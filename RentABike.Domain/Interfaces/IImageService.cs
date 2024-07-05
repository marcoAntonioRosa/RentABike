using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;

namespace RentABike.Domain.Interfaces;

public interface IImageService
{
    Task<PutObjectResponse> UploadImageAsync(string id, IFormFile file);
    
    Task<GetObjectResponse> GetImageAsync(string id);
}