using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace SimpleLambda;

public class Function
{
  public string FunctionHandler(SampleRequest request, ILambdaContext context)
  {
    context.Logger.LogInformation($"Hello {request.Input} from c# simpleLambda");
    return request.Input.ToUpper();
  }
}

public class SampleRequest
{
  public string Input { get; set; } = default!;
}