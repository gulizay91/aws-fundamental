using Files.Api.Configurations;
using Files.Api.Configurations.Swagger;
using Files.Api.Middlewares;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine("Files.Api starting...");
ConfigureHostSettings(builder.Host);
Console.WriteLine("Configured Host Settings...");
ConfigurationSettings(builder.Configuration);
RegisterServices(builder.Services, builder.Configuration);
Console.WriteLine("Services Registered...");

var app = builder.Build();

ConfigureWebApplication(app);

app.Run();

void ConfigurationSettings(IConfigurationBuilder configurationBuilder)
{
  configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
  configurationBuilder.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true);
  configurationBuilder.AddEnvironmentVariables();
}

void ConfigureHostSettings(IHostBuilder hostBuilder)
{
  //https://docs.microsoft.com/en-us/aspnet/core/migration/50-to-60-samples?view=aspnetcore-6.0
  // Wait 30 seconds for graceful shutdown.
  hostBuilder.ConfigureHostOptions(o => o.ShutdownTimeout = TimeSpan.FromSeconds(30));
}

void RegisterServices(IServiceCollection serviceCollection, IConfiguration configurationRoot)
{
  serviceCollection.AddHealthChecks();
  serviceCollection.RegisterLoggers(configurationRoot);
  serviceCollection.RegisterSwagger();
  serviceCollection.RegisterControllers();
  serviceCollection.RegisterServices(configurationRoot);
  serviceCollection.RegisterSettings(configurationRoot);
}

void ConfigureWebApplication(IApplicationBuilder applicationBuilder)
{
  var provider = applicationBuilder.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
  applicationBuilder.UseHttpsRedirection();
  applicationBuilder
    .UseMiddleware<
      ExceptionHandlerMiddleware>();
  applicationBuilder.UseRouting();
  applicationBuilder.UseEndpoints(endpoints => { endpoints.MapControllers(); });
  applicationBuilder.RegisterHealthCheck();
  applicationBuilder.UseSwagger();
  applicationBuilder.UseSwaggerUI(options =>
  {
    // build a swagger endpoint for each discovered Api version
    foreach (var description in provider.ApiVersionDescriptions)
      options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
        description.GroupName.ToUpperInvariant());
  });
}