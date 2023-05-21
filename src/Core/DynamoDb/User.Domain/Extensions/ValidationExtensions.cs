using FluentValidation.Results;

namespace User.Domain.Extensions;

public static class ValidationExtensions
{
  public static IEnumerable<ValidationFailure> GenerateValidationError(string paramName, string message)
  {
    return new[]
    {
      new ValidationFailure(paramName, message)
    };
  }
}