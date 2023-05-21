using System.Text.Json.Serialization;

namespace User.Domain.Entities;

public class UserEntity : Entity
{
  [JsonPropertyName("pk")] public string Pk => $"{ExternalAccount}#{ExternalAccountId}";

  [JsonPropertyName("sk")] public string Sk => EmailAddress;

  public required string UserName { get; init; }
  public required string ExternalAccount { get; init; }
  public required string ExternalAccountId { get; init; }
  public required string EmailAddress { get; init; }
  public DateTime BirthDate { get; init; }
  public string AvatarCode { get; init; }
  public DateTime UpdatedAt { get; set; }
}