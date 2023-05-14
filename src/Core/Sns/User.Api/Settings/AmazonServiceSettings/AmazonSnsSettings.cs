namespace User.Api.Settings.AmazonServiceSettings;

public record AmazonSnsSettings
{
  public string Region { get; set; }
  public Topics Topics { get; set; }
}