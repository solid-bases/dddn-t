using System.Linq.Expressions;

using DDDnt.DomainDrivenDesign.Specifications.Persistency.Components;
using DDDnt.DomainDrivenDesign.Storage;
using DDDnt.DomainDrivenDesign.ValueObjects;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Moq;

namespace DDDnt.DomainDrivenDesign.Specifications.CommandsHandler.Components;

public interface IHaveCommandsReceiverSteps : IHaveStepsWithContext<CommandsHandlerContext>
{
    void Given_the_logger_mock()
    {
        Context.LoggerMock = new Mock<ILogger<TestCommandsReceiver>>();
    }

    void Given_the_ServiceProvider_mock()
    {
        var services = Context.Services!;
        services.AddSingleton(Context.LoggerMock!.Object);
        var serviceProvider = services.BuildServiceProvider();

        Context.ScopeFactoryMock = new Mock<IServiceScopeFactory>();
        Context.ServiceProvider = serviceProvider;
        var scopeMock = new Mock<IServiceScope>();
        scopeMock.Setup(s => s.ServiceProvider)
            .Returns(() => Context.ServiceProvider);

        Context.ScopeFactoryMock.Setup(s => s.CreateScope())
            .Returns(() => scopeMock.Object);
    }

    void Given_the_TestCommandHandler()
    {
        Context.CommandsHandler = new TestCommandsReceiver(Context.ScopeFactoryMock!.Object);
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

    Expression<Action<ILogger<TestCommandsReceiver>>> LogInfoExp(LogLevel level, string? message = default)
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
        try
        {
            Context.CommandsHandler!.Handle(Context.Command!);
        }
        catch (Exception)
        {
        }
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
        Assert.True(Context.CommandsHandler!.ExecuteHasBeenCalled);
    }

    void Given_the_get_required_service_mock()
    {
        Context.Services = new ServiceCollection();
        Context.Services!.AddSingleton<ITestRepository, TestRepository>();
        Context.Services!.AddSingleton<IAnotherTestRepository, AnotherTestRepository>();
        Context.Services!.AddSingleton<ITestPublisher, TestPublisher>();
        Context.Services!.AddSingleton(new Mock<IEventStore<TestAggregate, AggregateId>>().Object);
    }

    void Then_the_repositories_are_injected()
    {
        // Context.ServiceProvider!.Verify(s => s.GetService(typeof(ITestRepository)), Times.Once());
        Assert.IsType<TestRepository>(Context.CommandsHandler!.GetRepository<ITestRepository>());

        // Context.ServiceProvider!.Verify(s => s.GetService(typeof(IAnotherTestRepository)), Times.Once());
        Assert.IsType<AnotherTestRepository>(Context.CommandsHandler!.GetRepository<IAnotherTestRepository>());
    }

    void Then_the_publishers_are_injected()
    {
        // Context.ServiceProvider!.Verify(s => s.GetService(typeof(ITestPublisher)), Times.Once());
        Assert.IsType<TestPublisher>(Context.CommandsHandler!.GetPublisher<ITestPublisher>());
    }

    void Then_TestCommandHandler_contains_two_delegates()
    {
        Assert.Equal(2, Context.CommandsHandler!.Delegates.Count);
    }
}
