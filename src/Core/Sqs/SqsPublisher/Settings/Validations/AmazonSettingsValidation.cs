using System.Text.Json;
using System.Text.Json.Serialization;
using Infrastructure.Loggers;
using Microsoft.Extensions.Options;
using SqsPublisher.Settings.AmazonServiceSettings;

namespace SqsPublisher.Settings.Validations;

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

    if (options.AmazonSqsSettings.GetType() != typeof(AmazonSqsSettings))
      return ValidateOptionsResult.Fail(
        $"{nameof(AmazonSettings)}:{typeof(AmazonSqsSettings)} is null");

    var amazonSqsQueues = options.AmazonSqsSettings.SqsQueues;

    if (amazonSqsQueues.GetType() != typeof(SqsQueues))
      return ValidateOptionsResult.Fail(
        $"{nameof(AmazonSettings)}:{typeof(AmazonSqsSettings)}:{typeof(SqsQueues)} is null");

    if (amazonSqsQueues.UserQueue is not null) return ValidateOptionsResult.Success;

    var message =
      $"{nameof(AmazonSettings)}:{typeof(AmazonSqsSettings)}:{typeof(SqsQueues)}:{nameof(amazonSqsQueues.UserQueue)} is not set";
    _logger.LogError(message);
    return ValidateOptionsResult.Fail(message);
  }
}