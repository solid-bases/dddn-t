using DDDnt.DomainDrivenDesign.Exceptions;
using DDDnt.DomainDrivenDesign.ValueObjects;

using Newtonsoft.Json;

namespace DDDnt.DomainDrivenDesign.Aggregate;

public abstract class AggregateRoot<TId>
    where TId : AggregateId
{
    [JsonIgnore]
    public abstract DomainEventsDelegates DomainEventsCollection { get; }

    public abstract TId? Id { get; set; }

    protected AggregateRoot()
    {
    }

    protected ICollection<IDomainEvent> _uncommittedEvents = [];

    public void ApplyEvent(IDomainEvent @event)
    {
        DomainEventsCollection.TryGetValue(@event.GetType(), out var apply);
        if (apply is null)
        {
            throw new ApplyDelegateNotImplementedException(@event.GetType().FullName, "Apply method not implemented");
        }
        apply.Invoke(@event);
    }

    public virtual void RaiseEvent(IDomainEvent @event)
    {
        _uncommittedEvents.Add(@event);
        ApplyEvent(@event);
    }

    public virtual ICollection<IDomainEvent> ClearUncommittedEvents()
    {
        var events = _uncommittedEvents.ToList();
        _uncommittedEvents.Clear();
        return events;
    }
}

