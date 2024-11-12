using DDDnt.DomainDrivenDesign.Aggregate;

namespace DDDnt.DomainDrivenDesign.EventPublisher;


public delegate void ExecuteDelegate(IAggregateEvent @event);

public class EventsDelegates : Dictionary<Type, ExecuteDelegate>
{
}
