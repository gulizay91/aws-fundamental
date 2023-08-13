namespace Files.Api.Settings.AWSSettings;

public record AmazonS3Settings
{
  public string BucketName { get; set; }
  public string Path { get; set; }
}