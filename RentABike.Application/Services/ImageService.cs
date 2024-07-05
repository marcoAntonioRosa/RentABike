using System.Reflection.Metadata;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RentABike.Domain.Interfaces;

namespace RentABike.Application.Services;

public class ImageService(IAmazonS3 s3, IConfiguration configuration) : IImageService
{
    private readonly string _bucketName = configuration.GetSection("AWS:BucketName").Value ?? throw new InvalidOperationException();

    public async Task<PutObjectResponse> UploadImageAsync(string id, IFormFile file)
    {
        if (!string.Equals(file.ContentType, "image/png") && !string.Equals(file.ContentType, "image/bmp"))
            throw new ImageFormatLimitationException("Only .png and .bmp image files are allowed");
        
        var putObjectRequest = new PutObjectRequest()
        {
            BucketName = _bucketName,
            Key = $"images/{id}",
            ContentType = file.ContentType,
            InputStream = file.OpenReadStream()
        };
        
        return await s3.PutObjectAsync(putObjectRequest);
    }

    public async Task<GetObjectResponse> GetImageAsync(string id)
    {
        var getObjectRequest = new GetObjectRequest()
        {
            BucketName = _bucketName,
            Key = $"images/{id}"
        };

        return await s3.GetObjectAsync(getObjectRequest);
    }
}