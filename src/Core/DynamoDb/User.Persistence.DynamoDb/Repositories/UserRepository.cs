using System.Net;
using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using User.Domain.Entities;
using User.Domain.Repositories;

namespace User.Persistence.DynamoDb.Repositories;

public class UserRepository : IUserRepository
{
  private const string TableName = "users";
  private const string PkSeperator = "#";
  private readonly IAmazonDynamoDB _dynamoDb;

  public UserRepository(IAmazonDynamoDB dynamoDb)
  {
    _dynamoDb = dynamoDb;
  }

  public async Task<bool> CreateAsync(UserEntity user)
  {
    user.UpdatedAt = DateTime.UtcNow;
    var customerAsJson = JsonSerializer.Serialize(user);
    var customerAsAttributes = Document.FromJson(customerAsJson).ToAttributeMap();

    var createItemRequest = new PutItemRequest
    {
      TableName = TableName,
      Item = customerAsAttributes,
      ConditionExpression = "attribute_not_exists(pk) and attribute_not_exists(sk)"
    };

    var response = await _dynamoDb.PutItemAsync(createItemRequest);
    return response.HttpStatusCode == HttpStatusCode.OK;
  }

  public async Task<UserEntity?> GetAsync(string externalAccount, string externalAccountId, string email)
  {
    var getItemRequest = new GetItemRequest
    {
      TableName = TableName,
      Key = new Dictionary<string, AttributeValue>
      {
        { "pk", new AttributeValue { S = $"{externalAccount}{PkSeperator}{externalAccountId}" } },
        { "sk", new AttributeValue { S = email } }
      }
    };

    var response = await _dynamoDb.GetItemAsync(getItemRequest);
    if (response.Item.Count == 0) return null;

    var itemAsDocument = Document.FromAttributeMap(response.Item);
    return JsonSerializer.Deserialize<UserEntity>(itemAsDocument.ToJson());
  }

  /// <summary>
  ///   LSI (Local Secondary Index) :
  ///   * must be partition and sort key
  ///   * must be create when table create (eventual and strong consistency)
  ///   * querying items with different sorting order of attributes, pk must be same.
  ///   * max: 5
  ///   GSI (Global Secondary Index):
  ///   * simple pk or pk+sk
  ///   * anytime create (eventual consistency)
  ///   * querying non-primary key attribute of a table, pk or sk can be any attribute on table.
  ///   * max: 20
  /// </summary>
  /// <param name="userName"></param>
  /// <returns></returns>
  public async Task<UserEntity?> GetByUserNameAsync(string userName)
  {
    var queryRequest = new QueryRequest
    {
      TableName = TableName,
      IndexName = "username-index", // (gsi)global secondary index
      KeyConditionExpression = "UserName = :v_UserName",
      ExpressionAttributeValues = new Dictionary<string, AttributeValue>
      {
        {
          ":v_UserName", new AttributeValue { S = userName }
        }
      }
    };

    var response = await _dynamoDb.QueryAsync(queryRequest);
    if (response.Items.Count == 0) return null;

    var itemAsDocument = Document.FromAttributeMap(response.Items[0]);
    return JsonSerializer.Deserialize<UserEntity>(itemAsDocument.ToJson());
  }

  public async Task<IEnumerable<UserEntity>> GetAllAsync()
  {
    var scanRequest = new ScanRequest
    {
      TableName = TableName
    };
    var response = await _dynamoDb.ScanAsync(scanRequest);
    return response.Items.Select(x =>
    {
      var json = Document.FromAttributeMap(x).ToJson();
      return JsonSerializer.Deserialize<UserEntity>(json);
    })!;
  }

  public async Task<bool> UpdateAsync(UserEntity user, DateTime requestStarted)
  {
    user.UpdatedAt = DateTime.UtcNow;
    var entityAsJson = JsonSerializer.Serialize(user);
    var entityAsAttributes = Document.FromJson(entityAsJson).ToAttributeMap();

    var updateItemRequest = new PutItemRequest
    {
      TableName = TableName,
      Item = entityAsAttributes,
      // https://dynobase.dev/dynamodb-locking/#:~:text=Optimistic%20Locking%20is%20a%20locking,before%20you%20save%20your%20changes.
      ConditionExpression = "UpdatedAt < :requestStarted", // optimistic concurrency
      ExpressionAttributeValues = new Dictionary<string, AttributeValue>
      {
        { ":requestStarted", new AttributeValue { S = requestStarted.ToString("O") } }
      }
    };

    var response = await _dynamoDb.PutItemAsync(updateItemRequest);
    return response.HttpStatusCode == HttpStatusCode.OK;
  }

  // public async Task<bool> DeleteAsync(Guid id)
  // {
  //     var deletedItemRequest = new DeleteItemRequest
  //     {
  //         TableName = TableName,
  //         Key = new Dictionary<string, AttributeValue>
  //         {
  //             { "pk", new AttributeValue { S = id.ToString() } },
  //             { "sk", new AttributeValue { S = id.ToString() } }
  //         }
  //     };
  //
  //     var response = await _dynamoDb.DeleteItemAsync(deletedItemRequest);
  //     return response.HttpStatusCode == HttpStatusCode.OK;
  // }
}