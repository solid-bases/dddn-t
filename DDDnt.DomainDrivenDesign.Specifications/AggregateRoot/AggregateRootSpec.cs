using DDDnt.DomainDrivenDesign.Exceptions;
using DDDnt.DomainDrivenDesign.Specifications.AggregateRoot.Components;

namespace DDDnt.DomainDrivenDesign.Specifications.AggregateRoot;

public class AggregateRootSpec : SpecificationsWithSteps<IHaveAggregateRootSteps, AggregateRootSteps, AggregateRootContext>
{
    public AggregateRootSpec(AggregateRootSteps steps) : base(steps)
    {
        void Background()
        {
            _steps.Given_the_TestAggregateRoot();
        }
        steps.InitializeContext(Background);
    }

    [Fact]
    public void RaiseEvent_should_add_the_event_to_uncommitted_events_collection()
    {
        _steps.Given_the_TestEvent();
        _steps.When_RaiseEvent_is_called();
        _steps.Then_the_uncommitted_events_should_contain(_steps.Context.Event!);
    }

    [Fact]
    public void RaiseEvent_should_call_ApplyEvent()
    {
        _steps.Given_the_TestEvent();
        _steps.When_RaiseEvent_is_called();
        _steps.Then_the_ApplyEvent_method_is_called();
    }

    [Fact()]
    public void ApplyEvent_should_call_the_specific_apply_event()
    {
        _steps.Given_the_TestEvent();
        _steps.When_ApplyEvent_is_called();
        _steps.Then_the_specific_apply_event_should_be_called();
    }

    [Fact]
    public void ApplyEvent_should_throw_if_no_apply_event_is_provided()
    {
        _steps.Given_the_TestEventWithNoApply();
        _steps.Then_the_exception_is_thrown<ApplyDelegateNotImplementedException>(
            _steps.When_ApplyEvent_is_called,
            $"Apply method not implemented (Parameter '{_steps.Context.Event!.GetType().FullName}')"
        );
    }

    [Fact]
    public void ClearUncommittedEvents_should_return_the_uncommitted_events_and_clear_the_list()
    {
        _steps.Given_the_TestEvent();
        _steps.Given_RaiseEvent_is_called();
        _steps.When_ClearUncommittedEvents_is_called();
        _steps.Then_the_uncommitted_events_should_be_empty();
        _steps.Then_ClearUncommittedEvents_returns_the_uncommitted_events();
    }
}
