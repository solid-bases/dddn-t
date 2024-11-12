using DDDnt.DomainDrivenDesign.Specifications.Publisher.Components;

using Microsoft.Extensions.Logging;

namespace DDDnt.DomainDrivenDesign.Specifications.Publisher;

public class PublisherSpec : SpecificationsWithSteps<IHavePublisherSteps, PublisherSteps, PublisherContext>
{
    public PublisherSpec(PublisherSteps steps) : base(steps)
    {
        void Background()
        {
            _steps.Given_the_logger_mock();
            // _steps.Given_the_get_required_service_mock();
            _steps.Given_the_TestPublisher();
            _steps.Given_the_TestEvent();
        }
        steps.InitializeContext(Background);
    }

    [Fact]
    public void Publish_should_log_integration_event_received()
    {
        _steps.Given_the_log_mocked(LogLevel.Information);
        _steps.When_Publish_is_called();
        _steps.Then_integration_event_received_should_be_logged(LogLevel.Information);
    }

    [Fact]
    public void Publish_should_call_Publish_method()
    {
        _steps.Given_the_log_mocked(LogLevel.Information);
        _steps.When_Publish_is_called();
        _steps.Then_Execute_is_called();
    }

    [Fact]
    public void Publish_should_log_exception()
    {
        _steps.Given_the_TestPublisherWithException();
        _steps.Given_the_log_mocked(LogLevel.Information);
        _steps.Given_the_log_mocked(LogLevel.Error);
        _steps.Given_an_execution_exception();
        _steps.When_Publish_is_called();
        _steps.Then_Publisher_contains_three_delegates();
        _steps.Then_integration_event_received_should_be_logged(LogLevel.Error);
    }

    [Fact]
    public void Publish_should_log_exception_when_no_delegate_is_defined()
    {
        _steps.Given_the_TestPublisherNoHandler();
        _steps.Given_the_log_mocked(LogLevel.Information);
        _steps.Given_the_log_mocked(LogLevel.Error);
        _steps.When_Publish_is_called();
        _steps.Then_integration_event_received_should_be_logged(LogLevel.Error);
    }

    [Fact]
    public void Publish_should_call_execute_method()
    {
        _steps.Given_the_TestDomainEvent();
        _steps.When_Publish_is_called();
        _steps.Then_Execute_is_called();
    }

    // [Fact]
    // public void Should_allow_to_inject_repositories()
    // {
    //     _steps.Then_the_repositories_are_injected();
    // }

    // [Fact]
    // public void Should_allow_to_inject_publishers()
    // {
    //     _steps.Then_the_publishers_are_injected();
    // }

}

