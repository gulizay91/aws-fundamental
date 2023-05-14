using MediatR;

namespace Contracts.Common;

public interface IContract : IRequest
{
  public Guid CorrelationId { get; set; }
}