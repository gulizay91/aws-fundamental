using Amazon.SimpleNotificationService.Model;
using Contracts.Common;
using Infrastructure.Constants;

namespace Infrastructure.Extensions;

public static class SnsContractExtensions
{
  private const string StringDataType = "String";

  public static Dictionary<string, MessageAttributeValue> GetSnsContractMessageAttributes<T>(this T contract)
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
}