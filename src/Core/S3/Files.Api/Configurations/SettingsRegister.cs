using Files.Api.Settings.AWSSettings;
using Files.Api.Settings.Validations;
using Microsoft.Extensions.Options;

namespace Files.Api.Configurations;

public static class SettingsRegister
{
  public static void RegisterSettings(this IServiceCollection serviceCollection, IConfiguration configuration)
  {
    //Add Options Validators
    serviceCollection.AddOptions<AmazonSettings>().ValidateOnStart();

    // register settings objects
    serviceCollection.Configure<AmazonSettings>(options =>
    {
      configuration.GetSection(nameof(AmazonSettings)).Bind(options);

      options.AddAmazonS3Settings(configuration);
    });

    //Options Validators
    serviceCollection.AddSingleton<IValidateOptions<AmazonSettings>, AmazonSettingsValidation>();
  }

  private static void AddAmazonS3Settings(this AmazonSettings options, IConfiguration configuration)
  {
    options.AmazonS3Settings = configuration
      .GetSection("AmazonSettings:AmazonS3Settings")
      .Get<AmazonS3Settings>()!;
  }
}