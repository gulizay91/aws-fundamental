using System.Text.Json;
using System.Text.Json.Serialization;
using Infrastructure.Loggers;
using Microsoft.Extensions.Options;
using User.Api.Settings.AmazonServiceSettings;

namespace User.Api.Settings.Validations;

public class AmazonSettingsValidation : IValidateOptions<AmazonSettings>
{
  private readonly JsonSerializerOptions _jsonSerializerOptions;
  private readonly IConsoleLogger _logger;

  public AmazonSettingsValidation(IConsoleLogger logger)
  {
    _logger = logger;
    _jsonSerializerOptions = new JsonSerializerOptions
    {
      DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
      IncludeFields = true,
      WriteIndented = false
    };
  }

  public ValidateOptionsResult Validate(string name, AmazonSettings options)
  {
    _logger.LogTrace($"{nameof(AmazonSettings)}:{JsonSerializer.Serialize(options, _jsonSerializerOptions)}");

    if (options.AmazonSnsSettings.GetType() != typeof(AmazonSnsSettings))
      return ValidateOptionsResult.Fail(
        $"{nameof(AmazonSettings)}:{typeof(AmazonSnsSettings)} is null");

    var amazonSnsTopics = options.AmazonSnsSettings.Topics;

    if (amazonSnsTopics.GetType() != typeof(Topics))
      return ValidateOptionsResult.Fail(
        $"{nameof(AmazonSettings)}:{typeof(AmazonSnsSettings)}:{typeof(Topics)} is null");

    if (amazonSnsTopics.TopicArn is not null) return ValidateOptionsResult.Success;

    var message =
      $"{nameof(AmazonSettings)}:{typeof(AmazonSnsSettings)}:{typeof(Topics)}:{nameof(amazonSnsTopics.TopicArn)} is not set";
    _logger.LogError(message);
    return ValidateOptionsResult.Fail(message);
  }
}