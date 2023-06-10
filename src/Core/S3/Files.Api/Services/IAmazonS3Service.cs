using Amazon.S3.Model;

namespace Files.Api.Services;

public interface IAmazonS3Service
{
  Task<PutObjectResponse> UploadFileAsync(string fileName, string filePath, IFormFile file);

  Task<GetObjectResponse> GetFileAsync(string fileName, string filePath);

  Task<DeleteObjectResponse> DeleteFileAsync(string fileName, string filePath);

  string? GetPreSignedUrlFromS3(string fileName, string filePath, int? expireMinutes = 5);
}