using System.Reflection;

using DDDnt.DomainDrivenDesign.Aggregate;
using DDDnt.DomainDrivenDesign.Storage;
using DDDnt.DomainDrivenDesign.ValueObjects;

using Newtonsoft.Json.Linq;

namespace DDDnt.DomainDrivenDesign.Persistency;

public abstract class EventRepository<T, TId>(IEventStore<T, TId> eventStore) : IEventRepository<T, TId> where TId : AggregateId where T : AggregateRoot<TId>, new()
{
    private readonly IEventStore<T, TId> _eventStore = eventStore;

    public async Task<T> GetByAggregateId(CorrelationId correlationId, TId id, CancellationToken cancellationToken = default)
    {
        var events = await _eventStore.ReadAggregate(new(), id, cancellationToken);
        if (events is null || events.Count == 0)
        {
            return new T();
        }

        var aggregate = new T();
        foreach (var @event in events)
        {
            IDomainEvent convertedEvent = ConvertEventType(@event);
            aggregate.ApplyEvent(convertedEvent);
        }
        return aggregate;

    }

    private static IDomainEvent ConvertEventType(JToken @event)
    {
        var eventName = @event["EventName"]?.ToString() ?? throw new MissingFieldException("EventName");
        Type type = GetTypeFromEventName(eventName);
        var convertedEvent = @event.ToObject(type) as IDomainEvent ?? throw new InvalidCastException(eventName);
        return convertedEvent;
    }

    private static Type GetTypeFromEventName(string eventName)
    {
        return eventName.StartsWith("Snapshot-")
            ? typeof(Snapshot<T, TId>)
            : Assembly.GetAssembly(typeof(T))?.GetType(eventName) ?? throw new TypeLoadException(eventName);
    }

    public Task Commit(CorrelationId correlationId, T aggregate, CancellationToken cancellationToken = default)
    {
        return _eventStore.Commit(new(), aggregate, cancellationToken);
    }

    public Task Snapshot(CorrelationId correlationId, T aggregate, CancellationToken cancellationToken = default)
    {
        return _eventStore.Snapshot(new(), aggregate, cancellationToken);
    }
}
