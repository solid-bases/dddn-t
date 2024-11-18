using DDDnt.DomainDrivenDesign.Aggregate;
using DDDnt.DomainDrivenDesign.ValueObjects;

namespace DDDnt.DomainDrivenDesign.Specifications.AggregateRoot.Components;

public class TestAggregateRoot : AggregateRoot<AggregateId>
{
    public override DomainEventsDelegates DomainEventsCollection { get; }

    public TestAggregateRoot() : base()
    {
        DomainEventsCollection = new()
        {
            { typeof(TestEvent), @event => ApplyTestEvent((TestEvent)@event) }
        };
    }

    public bool ApplyTestEventCalled { get; private set; } = false;

    internal void ApplyTestEvent(TestEvent _)
    {
        ApplyTestEventCalled = true;
    }
    public override AggregateId? Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public void PublicRaiseEvent(IDomainEvent @event) => base.RaiseEvent(@event);

}

internal record TestEvent(CorrelationId CorrelationId, User User) : IDomainEvent
{
    public string EventName => GetType().FullName ?? GetType().Name;
    public TestEvent() : this(new CorrelationId(), new("No User")) { }
}

internal record TestEventWithNoApply(CorrelationId CorrelationId, User User) : IDomainEvent
{
    public string EventName => GetType().FullName ?? GetType().Name;
    public TestEventWithNoApply() : this(new CorrelationId(), new("No User")) { }
}
