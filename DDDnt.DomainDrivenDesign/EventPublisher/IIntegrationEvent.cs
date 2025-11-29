using DDDnt.DomainDrivenDesign.Aggregate;
using DDDnt.DomainDrivenDesign.ValueObjects;

namespace DDDnt.DomainDrivenDesign.EventPublisher;

public interface IIntegrationEvent : IEvent
{
    CorrelationId CorrelationId { get; init; }
    User User { get; init; }
}
