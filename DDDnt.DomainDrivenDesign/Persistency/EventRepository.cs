using System.Reflection;

using Newtonsoft.Json.Linq;

using DDDnt.DomainDrivenDesign.Aggregate;
using DDDnt.DomainDrivenDesign.Storage;
using DDDnt.DomainDrivenDesign.ValueObjects;

namespace DDDnt.DomainDrivenDesign.Persistency;

public abstract class EventRepository : IEventRepository
{
    private readonly IEventStore _eventStore;

    protected EventRepository(IEventStore eventStore)
    {
        _eventStore = eventStore;
    }

    public async Task<T> GetByAggregateId<T>(CorrelationId correlationId, AggregateId id, CancellationToken cancellationToken = default) where T : AggregateRoot<AggregateId>, new()
    {
        var events = await _eventStore.ReadAggregate<T>(new(), id, cancellationToken);
        if (events is null || events.Count == 0)
        {
            return new T();
        }

        var aggregate = new T();
        foreach (var @event in events)
        {
            IDomainEvent convertedEvent = ConvertEventType<T>(@event);
            aggregate.ApplyEvent(convertedEvent);
        }
        return aggregate;

    }

    private static IDomainEvent ConvertEventType<T>(JToken @event) where T : AggregateRoot<AggregateId>, new()
    {
        var eventName = @event["EventName"]?.ToString() ?? throw new MissingFieldException("EventName");
        Type type = GetTypeFromEventName<T>(eventName);
        var convertedEvent = @event.ToObject(type) as IDomainEvent ?? throw new InvalidCastException(eventName);
        return convertedEvent;
    }

    private static Type GetTypeFromEventName<T>(string eventName) where T : AggregateRoot<AggregateId>, new()
    {
        return eventName.StartsWith("Snapshot-")
            ? typeof(Snapshot<T>)
            : Assembly.GetAssembly(typeof(T))?.GetType(eventName) ?? throw new TypeLoadException(eventName);
    }

    public Task Commit<T>(CorrelationId correlationId, T aggregate, CancellationToken cancellationToken = default) where T : AggregateRoot<AggregateId>
    {
        return _eventStore.Commit(new(), aggregate, cancellationToken);
    }

    public Task Snapshot<T>(CorrelationId correlationId, T aggregate, CancellationToken cancellationToken = default) where T : AggregateRoot<AggregateId>
    {
        return _eventStore.Snapshot(new(), aggregate, cancellationToken);
    }
}
