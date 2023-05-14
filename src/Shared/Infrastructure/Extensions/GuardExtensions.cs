namespace Infrastructure.Extensions;

public interface IGuard
{
}

public class Guard : IGuard
{
  private Guard()
  {
  }

  public static IGuard Against { get; } = new Guard();
}

public static class GuardExtensions
{
  public static void Null<T>(this IGuard guard, T value, string parameterName, string? message = null)
  {
    if (value == null)
      throw new ArgumentNullException(parameterName, message ?? $"Required value {parameterName} was null.");
  }

  public static string NullOrEmpty(this IGuard guard, string? value, string parameterName, string? message = null)
  {
    Guard.Against.Null(value, parameterName, message);
    if (value == string.Empty)
      throw new ArgumentException(message ?? $"Required value {parameterName} was empty.", parameterName);
    return value;
  }

  public static T NullOrDefault<T>(this IGuard guard, T value, string parameterName, string? message = null)
  {
    Guard.Against.Null(value, parameterName, message);
    if (value.Equals(default(T)))
      throw new ArgumentNullException(parameterName, message ?? $"Required value {parameterName} was default.");
    return value;
  }

  public static T NegativeOrZero<T>(this IGuard guard, T value, string parameterName) where T : struct, IComparable
  {
    if (value.CompareTo(default(T)) <= 0)
      throw new ArgumentException("Required value " + parameterName + " cannot be zero or negative.",
        parameterName);

    return value;
  }
}