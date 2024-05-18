namespace DDDnt.DomainDrivenDesign.Aggregate;

public delegate void ApplyDelegate(IDomainEvent @event);

public class DomainEventsDelegates : Dictionary<Type, ApplyDelegate>
{
}
