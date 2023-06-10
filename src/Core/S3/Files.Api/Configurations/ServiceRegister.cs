using Amazon.S3;
using Files.Api.Services;

namespace Files.Api.Configurations;

public static class ServiceRegister
{
  public static void RegisterServices(this IServiceCollection serviceCollection)
  {
    serviceCollection.AddSingleton<IAmazonS3, AmazonS3Client>();
    serviceCollection.AddScoped<IAmazonS3Service, AmazonS3Service>();
  }
}