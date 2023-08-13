namespace Files.Api.Settings.AWSSettings;

public record AmazonSettings
{
  public string? Region { get; set; }
  public string? AccountId { get; set; }
  public string? AccessKey { get; set; }
  public string? SecretKey { get; set; }
  public AmazonS3Settings AmazonS3Settings { get; set; }
}