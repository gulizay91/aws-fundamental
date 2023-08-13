using System.Text.Json;
using System.Text.Json.Serialization;
using Files.Api.Settings.AWSSettings;
using Microsoft.Extensions.Options;

namespace Files.Api.Settings.Validations;

public class AmazonSettingsValidation : IValidateOptions<AmazonSettings>
{
  private readonly JsonSerializerOptions _jsonSerializerOptions;
  private readonly ILogger<AmazonSettingsValidation> _logger;

  public AmazonSettingsValidation(ILogger<AmazonSettingsValidation> logger)
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

    if (options.AmazonS3Settings.GetType() != typeof(AmazonS3Settings))
      return ValidateOptionsResult.Fail(
        $"{nameof(AmazonSettings)}:{typeof(AmazonS3Settings)} is null");

    var amazonS3Bucket = options.AmazonS3Settings.BucketName;
    if (string.IsNullOrWhiteSpace(amazonS3Bucket))
      return ValidateOptionsResult.Fail(
        $"{nameof(AmazonSettings)}:{typeof(AmazonS3Settings)}:{nameof(amazonS3Bucket)} is null");

    var amazonS3Path = options.AmazonS3Settings.Path;
    if (string.IsNullOrWhiteSpace(amazonS3Path))
      return ValidateOptionsResult.Fail(
        $"{nameof(AmazonSettings)}:{typeof(AmazonS3Settings)}:{nameof(amazonS3Path)} is null");

    return ValidateOptionsResult.Success;
  }
}