namespace DDDnt.TestBase;

/// <summary>
/// Represents an interface for objects that have a step context of type T.
/// </summary>
/// <typeparam name="T">The type of the step context.</typeparam>
public interface IHaveStepsWithContext<T>
    where T : class, new()
{
    /// <summary>
    /// Gets the step context.
    /// </summary>
    T Context { get; }
}
