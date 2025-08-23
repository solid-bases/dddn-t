using DDDnt.DomainDrivenDesign.Aggregate;

namespace DDDnt.DomainDrivenDesign.Specifications.AggregateRoot.Components;

public interface IHaveAggregateRootSteps : IHaveStepsWithContext<AggregateRootContext>
{
    void Given_the_TestAggregateRoot()
    {
        Context.AggregateRoot = new TestAggregateRoot();
    }

    void Given_the_TestEvent()
    {
        Context.Event = new TestEvent();
    }

    void When_RaiseEvent_is_called()
    {
        Context.AggregateRoot!.PublicRaiseEvent(Context.Event!);
    }

    void Then_the_uncommitted_events_should_contain(IDomainEvent @event)
    {
        Assert.Contains(@event, Context.AggregateRoot!.ClearUncommittedEvents());
    }

    void Then_the_ApplyEvent_method_is_called()
    {
        Assert.True(Context.AggregateRoot!.ApplyTestEventCalled);
    }

    void When_ApplyEvent_is_called()
    {
        Context.AggregateRoot!.ApplyEvent(Context.Event!);
    }

    void Then_the_specific_apply_event_should_be_called()
    {
        Assert.True(Context.AggregateRoot!.ApplyTestEventCalled);
    }

    void Given_the_TestEventWithNoApply()
    {
        Context.Event = new TestEventWithNoApply();
    }

    void Then_the_exception_is_thrown<T>(Action when, string? message = default) where T : Exception
    {
        var exception = Assert.Throws<T>(when);
        if (message != null)
        {
            Assert.Equal(message, exception.Message);
        }
    }

    void Given_RaiseEvent_is_called() => When_RaiseEvent_is_called();
    void When_ClearUncommittedEvents_is_called()
    {
        Context.UncommittedEvents = Context.AggregateRoot!.ClearUncommittedEvents();
    }
    void Then_the_uncommitted_events_should_be_empty()
    {
        Assert.Empty(Context.AggregateRoot!.ClearUncommittedEvents());
    }
    void Then_ClearUncommittedEvents_returns_the_uncommitted_events()
    {
        Assert.NotEmpty(Context.UncommittedEvents!);
        Assert.Contains(Context.Event!, Context.UncommittedEvents!);
    }
}
