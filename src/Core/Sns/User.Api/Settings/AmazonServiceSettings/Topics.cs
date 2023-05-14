using Infrastructure.Constants;

namespace User.Api.Settings.AmazonServiceSettings;

public record Topics
{
  public TopicArn? TopicArn { get; set; }
}

public record TopicArn
{
  public string Name => QueueNames.UserQueueName;
}