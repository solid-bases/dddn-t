using System.Linq.Expressions;

using DDDnt.DomainDrivenDesign.Storage;

using Microsoft.Extensions.Logging;

using Moq;

namespace DDDnt.DomainDrivenDesign.Specifications.CommandsHandler.Components;

public interface IHaveCommandsHandlerSteps : IHaveStepsWithContext<CommandsHandlerContext>
{
    void Given_the_logger_mock()
    {
        Context.LoggerMock = new Mock<ILogger<TestCommandsHandler>>();
    }

    void Given_the_ServiceProvider_mock()
    {
        Context.ServiceProviderMock = new Mock<IServiceProvider>();
    }

    void Given_the_TestCommandHandler()
    {
        Context.CommandsHandler = new TestCommandsHandler(Context.LoggerMock!.Object, Context.ServiceProviderMock!.Object);
    }

    void Given_the_TestCommand()
    {
        Context.Command = new TestCommand();
    }

    void Given_the_TestCommandWithException()
    {
        Context.Command = new TestCommandWithException();
    }

    void Given_the_log_mocked(LogLevel level, string? message = default)
    {
        Context.LoggerMock!.Setup(LogInfoExp(level, message));
    }

    Expression<Action<ILogger<TestCommandsHandler>>> LogInfoExp(LogLevel level, string? message = default)
    {
        return l => l.Log(
                    level,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => message == null || v.ToString() == message),
                    It.IsAny<Exception?>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)
                );
    }

    void When_Handle_is_called()
    {
        Context.CommandsHandler!.Handle(Context.Command!);
    }

    void Then_command_received_should_be_logged(LogLevel level, string? message = default)
    {
        Context.LoggerMock!.Verify(LogInfoExp(level, message), Times.Once());
    }

    void Given_an_execution_exception()
    {
        Context.CommandsHandler!.Delegates.Add(typeof(TestCommandWithException), command => throw new NotImplementedException());
    }

    void Given_the_TestCommandNoHandler()
    {
        Context.Command = new TestCommandNoHandler();
    }

    void Then_Execute_is_called()
    {
        Context.CommandsHandler!.ExecuteHasBeenCalled.Should().BeTrue();
    }

    void Given_the_get_required_service_mock()
    {
        Context.ServiceProviderMock!
            .Setup(s => s.GetService(typeof(ITestRepository)))
            .Returns(new TestRepository(new Mock<IEventStore>().Object));
        Context.ServiceProviderMock!
            .Setup(s => s.GetService(typeof(IAnotherTestRepository)))
            .Returns(new AnotherTestRepository(new Mock<IEventStore>().Object));
        Context.ServiceProviderMock!
            .Setup(s => s.GetService(typeof(ITestPublisher)))
            .Returns(new TestPublisher());
    }

    void Then_the_repositories_are_injected()
    {
        Context.ServiceProviderMock!.Verify(s => s.GetService(typeof(ITestRepository)), Times.Once());
        Context.CommandsHandler!.GetRepository<ITestRepository>().Should().BeOfType<TestRepository>();

        Context.ServiceProviderMock!.Verify(s => s.GetService(typeof(IAnotherTestRepository)), Times.Once());
        Context.CommandsHandler!.GetRepository<IAnotherTestRepository>().Should().BeOfType<AnotherTestRepository>();
    }

    void Then_the_publishers_are_injected()
    {
        Context.ServiceProviderMock!.Verify(s => s.GetService(typeof(ITestPublisher)), Times.Once());
        Context.CommandsHandler!.GetPublisher<ITestPublisher>().Should().BeOfType<TestPublisher>();
    }

    void Then_TestCommandHandler_contains_two_delegates()
    {
        Context.CommandsHandler!.Delegates.Count.Should().Be(2);
    }
}
