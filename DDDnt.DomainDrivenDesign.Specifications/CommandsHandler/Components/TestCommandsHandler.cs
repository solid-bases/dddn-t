using Microsoft.Extensions.Logging;

using DDDnt.DomainDrivenDesign.Command;
using DDDnt.DomainDrivenDesign.Persistency;
using DDDnt.DomainDrivenDesign.Storage;
using DDDnt.DomainDrivenDesign.EventPublisher;

namespace DDDnt.DomainDrivenDesign.Specifications.CommandsHandler.Components;

public class TestCommandsHandler : Command.CommandsHandler
{
    public override RepositoryCollection RepositoriesTypes => [
        typeof(ITestRepository),
        typeof(IAnotherTestRepository)
    ];

    public override CommandsDelegates Delegates => new() { { typeof(TestCommand), command => Execute((TestCommand)command) } };

    public override PublisherCollection? PublishersTypes => [typeof(ITestPublisher)];

    public TestCommandsHandler(ILogger<TestCommandsHandler> logger, IServiceProvider serviceProvider) : base(logger, serviceProvider)
    {
    }

    public bool ExecuteHasBeenCalled { get; private set; } = false;


    public void Execute(TestCommand _)
    {
        ExecuteHasBeenCalled = true;
    }
}

internal class AnotherTestRepository(IEventStore store) : EventRepository(store), IAnotherTestRepository
{
}

internal interface IAnotherTestRepository : IEventRepository
{
}

internal class TestRepository(IEventStore store) : EventRepository(store), ITestRepository
{
}

internal interface ITestRepository : IEventRepository
{
}

public record TestCommandNoHandler : DomainCommand
{
}

public record TestCommand : DomainCommand
{
}

