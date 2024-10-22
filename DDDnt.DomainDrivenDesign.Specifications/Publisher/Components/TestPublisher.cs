using System.Diagnostics.CodeAnalysis;

using DDDnt.DomainDrivenDesign.EventPublisher;

using Microsoft.Extensions.Logging;

using PB = DDDnt.DomainDrivenDesign.EventPublisher;

namespace DDDnt.DomainDrivenDesign.Specifications.Publisher.Components;

public class TestPublisher : PB.Publisher
{
    public override required EventsDelegates Delegates { get; set; }

    [SetsRequiredMembers]
    public TestPublisher(ILogger<TestPublisher> logger) : base(logger)
    {
        Delegates = new() { { typeof(TestEvent), @event => Execute((TestEvent)@event) } };
    }


    public bool ExecuteHasBeenCalled { get; private set; } = false;

    public void Execute(TestEvent _)
    {
        ExecuteHasBeenCalled = true;
    }
}

public record TestEventNoHandler : IntegrationEvent
{
}

public record TestEvent : IntegrationEvent
{
}

