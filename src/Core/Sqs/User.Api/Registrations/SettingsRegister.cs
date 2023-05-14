using Microsoft.Extensions.Options;
using User.Api.Settings.AmazonServiceSettings;
using User.Api.Settings.Validations;

namespace User.Api.Registrations;

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