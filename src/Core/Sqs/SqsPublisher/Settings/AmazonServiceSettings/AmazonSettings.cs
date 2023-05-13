namespace SqsPublisher.Settings.AmazonServiceSettings;

public record AmazonSettings
{
  public string? Region { get; set; }
  public string? AccountId { get; set; }
  public string? AccessKey { get; set; }
  public string? SecretKey { get; set; }
  public AmazonSqsSettings AmazonSqsSettings { get; set; }
}