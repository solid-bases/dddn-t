using DDDnt.DomainDrivenDesign.Aggregate;
using DDDnt.DomainDrivenDesign.ValueObjects;

namespace DDDnt.DomainDrivenDesign.Persistency;

public record Snapshot<T, TId>(CorrelationId CorrelationId, User User, T Aggregate) : IDomainEvent
    where TId : AggregateId where T : AggregateRoot<TId>
{
    public string EventName => $"Snapshot-{typeof(T).Name}";
    public T AggregateData { get; init; } = Aggregate;
}
