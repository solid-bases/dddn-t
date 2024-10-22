using System.Linq.Expressions;

using Microsoft.Extensions.Logging;

using Moq;

namespace DDDnt.DomainDrivenDesign.Specifications.Publisher.Components;

public interface IHavePublisherSteps : IHaveStepsWithContext<PublisherContext>
{
    void Given_the_logger_mock()
    {
        Context.LoggerMock = new Mock<ILogger<TestPublisher>>();
    }

    void Given_the_TestPublisher()
    {
        Context.Publisher = new TestPublisher(Context.LoggerMock!.Object);
    }

    void Given_the_log_mocked(LogLevel level, string? message = default)
    {
        Context.LoggerMock!.Setup(LogInfoExp(level, message));
    }

    Expression<Action<ILogger<TestPublisher>>> LogInfoExp(LogLevel level, string? message = default)
    {
        return l => l.Log(
                    level,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => message == null || v.ToString() == message),
                    It.IsAny<Exception?>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)
                );
    }

    void Then_integration_event_received_should_be_logged(LogLevel level, string? message = default)
    {
        Context.LoggerMock!.Verify(LogInfoExp(level, message), Times.Once());
    }

    void Given_an_execution_exception()
    {
        Context.Publisher!.Delegates.Add(typeof(TestPublisherWithException), command => throw new NotImplementedException());
    }

    void Then_Execute_is_called()
    {
        Context.Publisher!.ExecuteHasBeenCalled.Should().BeTrue();
    }

    // void Given_the_get_required_service_mock()
    // {
    //     Context.ServiceProviderMock!
    //         .Setup(s => s.GetService(typeof(ITestRepository)))
    //         .Returns(new TestRepository(new Mock<IEventStore>().Object));
    // }

    void When_Publish_is_called()
    {
        Context.Publisher!.Publish(Context.IntegrationEvent!);
    }

    void Given_the_TestEvent()
    {
        Context.IntegrationEvent = new TestEvent();
    }

    void Given_the_TestPublisherWithException()
    {
        Context.IntegrationEvent = new TestPublisherWithException();
    }

    void Given_the_TestPublisherNoHandler()
    {
        Context.IntegrationEvent = new TestPublisherWithException();
    }

    void Then_Publisher_contains_two_delegates()
    {
        Context.Publisher!.Delegates.Count.Should().Be(2);
    }
}
