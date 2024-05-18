using DDDnt.DomainDrivenDesign.ValueObjects;

namespace DDDnt.DomainDrivenDesign.Command;

public record DomainCommand : ICommand
{
    public CorrelationId CorrelationId { get; init; }
    public User User { get; init; }

    public DomainCommand() : this(new(), new User("No User"))
    {
    }

    public DomainCommand(CorrelationId correlationId) : this(correlationId, new User("No User"))
    {
    }

    public DomainCommand(User user) : this(new(), user)
    {
    }

    public DomainCommand(CorrelationId correlationId, User user)
    {
        this.CorrelationId = correlationId;
        this.User = user;
    }
}
