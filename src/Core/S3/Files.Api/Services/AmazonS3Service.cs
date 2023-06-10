using Amazon.S3;
using Amazon.S3.Model;

namespace Files.Api.Services;

public class AmazonS3Service : IAmazonS3Service
{
  private const string BucketName = "themoonstudio-root";
  private readonly IAmazonS3 _s3;

  public AmazonS3Service(IAmazonS3 s3)
  {
    _s3 = s3;
  }

  public async Task<PutObjectResponse> UploadFileAsync(string fileName, string filePath, IFormFile file)
  {
    var putObjectRequest = new PutObjectRequest
    {
      BucketName = BucketName,
      Key = $"{filePath}/{fileName}",
      ContentType = file.ContentType,
      InputStream = file.OpenReadStream(),
      Metadata =
      {
        ["x-amz-meta-originalname"] = file.FileName,
        ["x-amz-meta-extension"] = Path.GetExtension(file.FileName)
      }
    };

    return await _s3.PutObjectAsync(putObjectRequest);
  }

  public async Task<GetObjectResponse> GetFileAsync(string fileName, string filePath)
  {
    var getObjectRequest = new GetObjectRequest
    {
      BucketName = BucketName,
      Key = $"{filePath}/{fileName}"
    };

    return await _s3.GetObjectAsync(getObjectRequest);
  }

  public async Task<DeleteObjectResponse> DeleteFileAsync(string fileName, string filePath)
  {
    var deleteObjectRequest = new DeleteObjectRequest
    {
      BucketName = BucketName,
      Key = $"{filePath}/{fileName}"
    };

    return await _s3.DeleteObjectAsync(deleteObjectRequest);
  }

  public string? GetPreSignedUrlFromS3(string fileName, string filePath, int? expireMinutes = 5)
  {
    // Create a CopyObject request
    var request = new GetPreSignedUrlRequest
    {
      BucketName = BucketName,
      Key = $"{filePath}/{fileName}",
      Expires = DateTime.Now.AddMinutes(expireMinutes!.Value)
    };

    // Get path for request
    var path = _s3.GetPreSignedURL(request);

    return path;
  }
}