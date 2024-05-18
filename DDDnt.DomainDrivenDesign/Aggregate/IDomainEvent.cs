using DDDnt.DomainDrivenDesign.ValueObjects;

namespace DDDnt.DomainDrivenDesign.Aggregate;

public interface IDomainEvent
{
    /// <summary>
    /// This should be manually implemented in each domain event class and
    /// should be <b>strictly equal</b> to 
    /// <code>public string EventName => GetType().FullName ?? GetType().Name;</code>
    /// </summary>
    string EventName { get; }
    CorrelationId CorrelationId { get; }
    User User { get; }
}
