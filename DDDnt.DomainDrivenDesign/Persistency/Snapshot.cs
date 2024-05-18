using DDDnt.DomainDrivenDesign.Aggregate;
using DDDnt.DomainDrivenDesign.ValueObjects;

namespace DDDnt.DomainDrivenDesign.Persistency;

public record Snapshot<T>(CorrelationId CorrelationId, User User, T Aggregate) : IDomainEvent
    where T : AggregateRoot<AggregateId>
{
    public string EventName => $"Snapshot-{typeof(T).Name}";
    public T AggregateData { get; init; } = Aggregate;
}
