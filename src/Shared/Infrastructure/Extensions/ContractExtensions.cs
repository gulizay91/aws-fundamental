using Amazon.SQS.Model;
using Contracts.Common;
using Infrastructure.Constants;

namespace Infrastructure.Extensions;

public static class ContractExtensions
{
  private const string StringDataType = "String";

  public static Dictionary<string, MessageAttributeValue> GetContractMessageAttributes<T>(this T contract)
    where T : IContract
  {
    return new Dictionary<string, MessageAttributeValue>
    {
      {
        ContractAttributes.MessageBaseType,
        new MessageAttributeValue { DataType = StringDataType, StringValue = typeof(IContract).FullName }
      },
      {
        ContractAttributes.MessageType,
        new MessageAttributeValue { DataType = StringDataType, StringValue = typeof(T).Name }
      },
      {
        ContractAttributes.MessageTypeFullName,
        new MessageAttributeValue { DataType = StringDataType, StringValue = typeof(T).FullName }
      },
      {
        ContractAttributes.CorrelationId,
        new MessageAttributeValue { DataType = StringDataType, StringValue = contract.CorrelationId.ToString() }
      }
    };
  }

  public static Dictionary<string, MessageAttributeValue> GetCommandMessageAttributesV0<T>(this T contract)
    where T : ICommand
  {
    return new Dictionary<string, MessageAttributeValue>
    {
      {
        ContractAttributes.MessageBaseType,
        new MessageAttributeValue { DataType = StringDataType, StringValue = typeof(IContract).FullName }
      },
      {
        ContractAttributes.MessageType,
        new MessageAttributeValue { DataType = StringDataType, StringValue = nameof(ICommand) }
      },
      {
        ContractAttributes.MessageTypeNamespace,
        new MessageAttributeValue { DataType = StringDataType, StringValue = typeof(ICommand).Namespace }
      },
      {
        ContractAttributes.MessageName,
        new MessageAttributeValue { DataType = StringDataType, StringValue = typeof(T).Name }
      },
      {
        ContractAttributes.MessageFullName,
        new MessageAttributeValue { DataType = StringDataType, StringValue = typeof(T).FullName }
      },
      {
        ContractAttributes.CorrelationId,
        new MessageAttributeValue { DataType = StringDataType, StringValue = contract.CorrelationId.ToString() }
      }
    };
  }

  public static Dictionary<string, MessageAttributeValue> GetEventMessageAttributes<T>(this T contract) where T : IEvent
  {
    return new Dictionary<string, MessageAttributeValue>
    {
      {
        ContractAttributes.MessageType, new MessageAttributeValue { DataType = "String", StringValue = nameof(IEvent) }
      },
      {
        ContractAttributes.MessageTypeNamespace,
        new MessageAttributeValue { DataType = "String", StringValue = typeof(IEvent).Namespace }
      },
      {
        ContractAttributes.MessageName, new MessageAttributeValue { DataType = "String", StringValue = typeof(T).Name }
      },
      {
        ContractAttributes.MessageFullName,
        new MessageAttributeValue { DataType = "String", StringValue = typeof(T).FullName }
      },
      {
        ContractAttributes.CorrelationId,
        new MessageAttributeValue { DataType = "String", StringValue = contract.CorrelationId.ToString() }
      }
    };
  }
}