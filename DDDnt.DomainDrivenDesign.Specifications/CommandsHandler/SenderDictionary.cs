using System.Linq.Expressions;

using Moq;

using DDDnt.DomainDrivenDesign.Command;
using DDDnt.DomainDrivenDesign.Sender;

namespace DDDnt.DomainDrivenDesign.Specifications.CommandsHandler;

/// <summary>
/// Represents a dictionary that maps listener types to their corresponding mock objects.
/// </summary>
public class SenderDictionary : Dictionary<Type, object>
{
    /// <summary>
    /// Adds a mock object for the specified listener type to the dictionary.
    /// </summary>
    /// <typeparam name="Sender">The type of the sender.</typeparam>
    public void Add<Sender>() where Sender : class, ISender
    {
        Add(typeof(Sender), new Mock<Sender>());
    }

    /// <summary>
    /// Retrieves the mock object for the specified listener type from the dictionary.
    /// </summary>
    /// <typeparam name="Sender">The type of the listener.</typeparam>
    /// <returns>The mock object for the specified listener type.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the mock object for the specified listener type is not found in the dictionary.</exception>
    public Mock<Sender> Get<Sender>() where Sender : class, ISender
    {
        return this[typeof(Sender)] is not Mock<Sender> mock
            ? throw new InvalidOperationException($"Mock for {typeof(Sender).Name} not found")
            : mock;
    }

    /// <summary>
    /// Retrieves an expression representing the handler method of the specified listener type.
    /// </summary>
    /// <typeparam name="Sender">The type of the listener.</typeparam>
    /// <param name="specificCommand">The specific command to be handled by the listener. If not provided, a default command is used.</param>
    /// <returns>An expression representing the handler method of the specified listener type.</returns>
    public static Expression<Func<Sender, Task>> GetHandler<Sender>(ICommand? specificCommand = default)
        where Sender : class, ISender
    {
        return specificCommand != null
            ? x => x.Send(specificCommand)
            : x => x.Send(It.IsAny<ICommand>());
    }
}
