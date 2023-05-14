using Microsoft.AspNetCore.Mvc.Filters;
using User.Api.Filters.ValidationModels;

namespace User.Api.Filters;

public class ValidationFilterAttribute : ActionFilterAttribute
{
  public override void OnActionExecuting(ActionExecutingContext context)
  {
    if (!context.ModelState.IsValid) context.Result = new ValidationFailedResult(context.ModelState);
  }
}