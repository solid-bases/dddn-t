using DDDnt.DomainDrivenDesign.Aggregate;

namespace DDDnt.DomainDrivenDesign.EventPublisher;


public delegate void ExecuteDelegate(IEvent @event);

public class EventsDelegates : Dictionary<Type, ExecuteDelegate>
{
}
