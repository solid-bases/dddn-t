using System.Diagnostics.CodeAnalysis;

using DDDnt.DomainDrivenDesign.Aggregate;
using DDDnt.DomainDrivenDesign.EventPublisher;
using DDDnt.DomainDrivenDesign.ValueObjects;

using Microsoft.Extensions.Logging;

using PB = DDDnt.DomainDrivenDesign.EventPublisher;

namespace DDDnt.DomainDrivenDesign.Specifications.Publisher.Components;

public class TestPublisher : PB.Publisher
{
    public override required EventsDelegates Delegates { get; init; }

    [SetsRequiredMembers]
    public TestPublisher(ILogger<TestPublisher> logger) : base(logger)
    {
        Delegates = new() {
            { typeof(TestIntegrationEvent), @event => Execute((TestIntegrationEvent)@event) },
            { typeof(TestDomainEvent), @event => Execute((TestDomainEvent)@event) }
            };
    }


    public bool ExecuteHasBeenCalled { get; private set; } = false;

    public void Execute(TestIntegrationEvent _)
    {
        ExecuteHasBeenCalled = true;
    }

    public void Execute(TestDomainEvent _)
    {
        ExecuteHasBeenCalled = true;
    }
}

public record TestEventNoHandler : IIntegrationEvent
{
    public required CorrelationId CorrelationId { get; init; }
    public required User User { get; init; }
}

public record TestIntegrationEvent : IIntegrationEvent
{
    public required CorrelationId CorrelationId { get; init; }
    public required User User { get; init; }
}

public record TestDomainEvent(CorrelationId CorrelationId, User User) : IIntegrationEvent
{
    public string EventName => "TestDomainEvent";
}
