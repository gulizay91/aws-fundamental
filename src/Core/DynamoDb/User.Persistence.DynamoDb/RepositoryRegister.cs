using Amazon.DynamoDBv2;
using Microsoft.Extensions.DependencyInjection;
using User.Domain.Repositories;
using User.Persistence.DynamoDb.Repositories;

namespace User.Persistence.DynamoDb;

public static class RepositoryRegister
{
  public static void RegisterRepository(this IServiceCollection serviceCollection)
  {
    serviceCollection.AddSingleton<IAmazonDynamoDB, AmazonDynamoDBClient>();
    serviceCollection.AddScoped<IUserRepository, UserRepository>();
  }
}