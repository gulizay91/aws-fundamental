using Infrastructure.Constants;

namespace User.Api.Settings.AmazonServiceSettings;

public record SqsQueues
{
  public UserQueue? UserQueue { get; set; }
}

public record UserQueue
{
  public string Name => QueueNames.UserQueueName;
}