namespace User.Domain.Entities;

public interface IEntity<out T> where T : struct, IComparable<T>, IEquatable<T>
{
  public T Id { get; }
}