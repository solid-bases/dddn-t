using System.Diagnostics.CodeAnalysis;

using DDDnt.DomainDrivenDesign.Command;
using DDDnt.DomainDrivenDesign.EventPublisher;
using DDDnt.DomainDrivenDesign.Persistency;
using DDDnt.DomainDrivenDesign.Storage;

using Microsoft.Extensions.Logging;

namespace DDDnt.DomainDrivenDesign.Specifications.CommandsHandler.Components;

public class TestCommandsHandler : Command.CommandsHandler
{
    public override required CommandsDelegates Delegates { get; init; }

    public override RepositoryCollection RepositoriesTypes => [
        typeof(ITestRepository),
        typeof(IAnotherTestRepository)
    ];

    public override PublisherCollection? PublishersTypes => [typeof(ITestPublisher)];

    [SetsRequiredMembers]
    public TestCommandsHandler(ILogger<TestCommandsHandler> logger, IServiceProvider serviceProvider) : base(logger, serviceProvider)
    {
        Delegates = new() { { typeof(TestCommand), @event => Execute((TestCommand)@event) } };
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

