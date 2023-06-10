using Amazon.S3;
using Files.Api.Services;

namespace Files.Api.Configurations;

public static class ServiceRegister
{
  public static void RegisterServices(this IServiceCollection serviceCollection, IConfiguration configuration)
  {
    var region = configuration.GetSection("AmazonSettings:Region").Value;
    var accessKey = configuration.GetSection("AmazonSettings:AccessKey").Value;
    var secretKey = configuration.GetSection("AmazonSettings:SecretKey").Value;
    //var bucketName = configuration.GetSection("AmazonSettings:AmazonS3Settings:BucketName").Value;
#if !DEBUG
        serviceCollection.AddTransient<IAmazonS3, AmazonS3Client>(_ => new AmazonS3Client(region));
#else
    //serviceCollection.AddTransient<IAmazonS3, AmazonS3Client>(_ => new AmazonS3Client(accessKey, secretKey, region));
    serviceCollection.AddTransient<IAmazonS3, AmazonS3Client>();
#endif

    serviceCollection.AddScoped<IAmazonS3Service, AmazonS3Service>();
  }
}