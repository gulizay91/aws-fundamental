using System.ComponentModel.DataAnnotations;
using System.Net;
using Amazon.S3.Model;
using Files.Api.Middlewares;
using Files.Api.Services;
using Files.Api.Settings.AWSSettings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Files.Api.V1.Controllers;

//[Produces(MediaTypeNames.Application.Json)]
//[Consumes(MediaTypeNames.Application.Json)]
[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
[ApiController]
[Route("api/v{version:apiVersion}/files")]
public class FilesController : ControllerBase
{
  private readonly IAmazonS3Service _amazonS3Service;
  private readonly ILogger<FilesController> _logger;
  private readonly IOptions<AmazonSettings> _options;

  public FilesController(ILogger<FilesController> logger, IAmazonS3Service amazonS3Service,
    IOptions<AmazonSettings> options)
  {
    _logger = logger;
    _amazonS3Service = amazonS3Service;
    _options = options;
  }

  [HttpPost("{fileName}")]
  [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(PutObjectResponse), StatusCodes.Status200OK)]
  public async Task<IActionResult> Upload([FromRoute] string fileName,
    [FromForm(Name = "Data")] IFormFile file)
  {
    var response = await _amazonS3Service.UploadFileAsync(fileName, _options.Value.AmazonS3Settings.Path, file);
    if (response.HttpStatusCode == HttpStatusCode.OK) return Ok(response);

    return BadRequest(response);
  }

  [HttpGet("{fileName}")]
  [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(PutObjectResponse), StatusCodes.Status200OK)]
  public async Task<IActionResult> Get([FromRoute] string fileName)
  {
    var response = await _amazonS3Service.GetFileAsync(fileName, _options.Value.AmazonS3Settings.Path);
    return File(response.ResponseStream, response.Headers.ContentType);
  }

  [HttpDelete("{fileName}")]
  [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status200OK)]
  public async Task<IActionResult> Delete([FromRoute] string fileName)
  {
    var response = await _amazonS3Service.DeleteFileAsync(fileName, _options.Value.AmazonS3Settings.Path);
    return response.HttpStatusCode switch
    {
      HttpStatusCode.NoContent => Ok(),
      HttpStatusCode.NotFound => NotFound(),
      _ => BadRequest()
    };
  }

  [HttpGet("{fileName}/presigned-url")]
  [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
  [ProducesResponseType(typeof(Uri), StatusCodes.Status200OK)]
  public IActionResult GetUrl([FromRoute] string fileName)
  {
    _logger.LogInformation(
      $"Your Bucket: {_options.Value.AmazonS3Settings.BucketName} and Path: {_options.Value.AmazonS3Settings.Path}");
    var response = _amazonS3Service.GetPreSignedUrlFromS3(fileName, _options.Value.AmazonS3Settings.Path);
    if (string.IsNullOrWhiteSpace(response))
      return NotFound();
    var url = new Uri(response, UriKind.Absolute);
    return Ok(url);
  }
}