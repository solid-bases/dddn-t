using DDDnt.DomainDrivenDesign.EventPublisher;
using DDDnt.DomainDrivenDesign.ValueObjects;

namespace DDDnt.DomainDrivenDesign.Specifications.Publisher.Components;

public record TestPublisherWithException : IIntegrationEvent
{
    public required CorrelationId CorrelationId { get; init; }
    public required User User { get; init; }
}
