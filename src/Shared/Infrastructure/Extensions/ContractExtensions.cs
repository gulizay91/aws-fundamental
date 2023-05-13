using Amazon.SQS.Model;
using Contracts.Common;

namespace Infrastructure.Extensions;

public static class ContractExtensions
{
  public static Dictionary<string, MessageAttributeValue> GetCommandMessageInfo<T>(this T contract) where T : ICommand
  {
    return new Dictionary<string, MessageAttributeValue>
    {
      {
        "MessageType", new MessageAttributeValue { DataType = "String", StringValue = nameof(ICommand) }
      },
      {
        "MessageName", new MessageAttributeValue { DataType = "String", StringValue = typeof(T).Name }
      },
      {
        "MessageFullName", new MessageAttributeValue { DataType = "String", StringValue = typeof(T).FullName }
      }
    };
  }

  public static Dictionary<string, MessageAttributeValue> GetEventMessageInfo<T>(this T contract) where T : IEvent
  {
    return new Dictionary<string, MessageAttributeValue>
    {
      {
        "MessageType", new MessageAttributeValue { DataType = "String", StringValue = nameof(IEvent) }
      },
      {
        "MessageName", new MessageAttributeValue { DataType = "String", StringValue = typeof(T).Name }
      },
      {
        "MessageFullName", new MessageAttributeValue { DataType = "String", StringValue = typeof(T).FullName }
      }
    };
  }
}