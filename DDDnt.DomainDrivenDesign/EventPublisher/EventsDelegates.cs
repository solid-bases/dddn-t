namespace DDDnt.DomainDrivenDesign.EventPublisher;

public delegate void ExecuteDelegate(IIntegrationEvent @event);

public class EventsDelegates : Dictionary<Type, ExecuteDelegate>
{
}
