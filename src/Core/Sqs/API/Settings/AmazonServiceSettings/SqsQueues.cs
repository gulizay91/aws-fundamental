using Infrastructure.Constants;

namespace API.Settings.AmazonServiceSettings;

public record SqsQueues
{
  public UserQueue? UserQueue { get; set; }
}

public record UserQueue
{
  public string Name => QueueNames.UserQueueName;
}