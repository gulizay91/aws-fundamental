using API.Settings.AmazonServiceSettings;
using API.Settings.Validations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace API.Installers;

public static class SettingsInstaller
{
  public static void InstallSettings(this IServiceCollection serviceCollection, IConfiguration configuration)
  {
    //Add Options Validators
    serviceCollection.AddOptions<AmazonSettings>().ValidateOnStart();

    // register settings objects
    serviceCollection.Configure<AmazonSettings>(options =>
    {
      configuration.GetSection(nameof(AmazonSettings)).Bind(options);

      options.AddAmazonSqsSettings(configuration);
    });

    //Options Validators
    serviceCollection.AddSingleton<IValidateOptions<AmazonSettings>, AmazonSettingsValidation>();
  }

  private static void AddAmazonSqsSettings(this AmazonSettings options, IConfiguration configuration)
  {
    options.AmazonSqsSettings = configuration
      .GetSection("AmazonSettings:AmazonSqsSettings")
      .Get<AmazonSqsSettings>()!;
  }
}