namespace Infrastructure.Constants;

public static class ContractAttributes
{
  public const string MessageBaseType = "MessageBaseType";
  public const string MessageType = "MessageType";
  public const string MessageTypeFullName = "MessageTypeFullName";
  public const string CorrelationId = "CorrelationId";

  [Obsolete] public const string MessageTypeNamespace = "MessageTypeNamespace";

  [Obsolete] public const string MessageName = "MessageName";

  [Obsolete] public const string MessageFullName = "MessageFullName";
}