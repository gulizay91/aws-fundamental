namespace API.Settings.AmazonServiceSettings;

public record AmazonSqsSettings
{
  public string Region { get; set; }
  public SqsQueues SqsQueues { get; set; }
}