using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Filters.ValidationModels;

public class ValidationErrorResponse
{
  public ValidationErrorResponse(ModelStateDictionary modelState)
  {
    Message = "Validation Failed";
    Errors = modelState.Keys
      .SelectMany(key => modelState[key]!.Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
      .ToList();
  }

  public string Message { get; }
  public List<ValidationError> Errors { get; }
}