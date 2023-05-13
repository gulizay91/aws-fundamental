// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SqsPublisher.Installers;

Console.WriteLine("Hello, Publisher!");

/*
// * Sample
var sqsClient = new AmazonSQSClient();
var queueUrlResponse = await sqsClient.GetQueueUrlAsync(QueueNames.UserQueueName);
var commandCreateUser = ContractObjectMother.SimpleFakeCreateUser();
var sendMessageRequest = new SendMessageRequest
{
  QueueUrl = queueUrlResponse.QueueUrl,
  MessageBody = JsonSerializer.Serialize(ContractObjectMother.SimpleFakeCreateUser()),
  MessageAttributes = commandCreateUser.GetCommandMessageInfo()
};
var sendResponse = await sqsClient.SendMessageAsync(sendMessageRequest);
Console.WriteLine(JsonSerializer.Serialize(sendResponse));
*/


var hostBuilder = new HostBuilder()
  .ConfigureHostConfiguration(configHost =>
    configHost.AddEnvironmentVariables("ASPNETCORE_")
  )
#if DEBUG
  .UseEnvironment("Local")
#endif
  .ConfigureAppConfiguration((hostingContext, config) =>
  {
    config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
    config.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true);
    config.AddEnvironmentVariables();
    Console.WriteLine($"{hostingContext.HostingEnvironment.EnvironmentName}");
  })
  .ConfigureServices(ConfigureServices)
  .ConfigureLogging((hostBuilderContext, loggingBuilder) =>
  {
    loggingBuilder.AddConfiguration(hostBuilderContext.Configuration.GetSection("Logging"));
    loggingBuilder.AddConsole();
  });

await hostBuilder.RunConsoleAsync();

void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
{
  services.RegisterLoggers(hostContext.Configuration);
  services.InstallSettings(hostContext.Configuration);
  services.RegisterHealthCheck();
  services.RegisterServices();
  ConfigureHostSettings(services);
}

void ConfigureHostSettings(IServiceCollection services)
{
  services.Configure<HostOptions>(opts => opts.ShutdownTimeout = TimeSpan.FromSeconds(45));
}