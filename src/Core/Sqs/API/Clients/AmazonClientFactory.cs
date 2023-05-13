using Amazon;
using Amazon.SQS;
using API.Settings.AmazonServiceSettings;
using Microsoft.Extensions.Options;

namespace API.Clients;

public class AmazonClientFactory : IAmazonClientFactory
{
  private static readonly RegionEndpoint BucketRegion = RegionEndpoint.USEast1;
  private readonly IOptions<AmazonSettings> _options;

  public AmazonClientFactory(IOptions<AmazonSettings> options)
  {
    _options = options;
  }

  public IAmazonSQS GetSqsClient()
  {
    //var sqsSettings = _options.Value.AmazonSqsSettings;
    var config = new AmazonSQSConfig
    {
      RegionEndpoint = RegionEndpoint.GetBySystemName(_options.Value.Region ?? BucketRegion.DisplayName),
      ServiceURL = $"https://sqs.{_options.Value.Region}.amazonaws.com"
    };

    return new AmazonSQSClient(_options.Value.AccessKey, _options.Value.SecretKey, config);
  }

  public string GetSqsQueue(string queueName)
  {
    return
      $"https://sqs.{_options.Value.Region ?? BucketRegion.DisplayName}.amazonaws.com/{_options.Value.AccountId}/{queueName}";
  }
}