namespace User.Domain.Entities;

public class Entity : IEntity<Guid>
{
  public Guid Id { get; init; } = default!;
}