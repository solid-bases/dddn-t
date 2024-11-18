using System.Diagnostics.CodeAnalysis;

using DDDnt.DomainDrivenDesign.Command;
using DDDnt.DomainDrivenDesign.EventPublisher;
using DDDnt.DomainDrivenDesign.Persistency;
using DDDnt.DomainDrivenDesign.Specifications.Persistency.Components;
using DDDnt.DomainDrivenDesign.Storage;
using DDDnt.DomainDrivenDesign.ValueObjects;

using Microsoft.Extensions.Logging;

namespace DDDnt.DomainDrivenDesign.Specifications.CommandsHandler.Components;

public class TestCommandsReceiver : Command.CommandsReceiver
{
    public override required CommandsDelegates Delegates { get; init; }

    public override RepositoryCollection RepositoriesTypes => [
        typeof(ITestRepository),
        typeof(IAnotherTestRepository)
    ];

    public override PublisherCollection? PublishersTypes => [typeof(ITestPublisher)];

    [SetsRequiredMembers]
    public TestCommandsReceiver(ILogger<TestCommandsReceiver> logger, IServiceProvider serviceProvider) : base(logger, serviceProvider)
    {
        Delegates = new() { { typeof(TestCommand), @event => Execute((TestCommand)@event) } };
    }

    public bool ExecuteHasBeenCalled { get; private set; } = false;

    public void Execute(TestCommand _)
    {
        ExecuteHasBeenCalled = true;
    }
}

internal class AnotherTestRepository(IEventStore<TestAggregate, AggregateId> store) : EventRepository<TestAggregate, AggregateId>(store), IAnotherTestRepository
{
}

internal interface IAnotherTestRepository : IEventRepository<TestAggregate, AggregateId>
{
}

internal class TestRepository(IEventStore<TestAggregate, AggregateId> store) : EventRepository<TestAggregate, AggregateId>(store), ITestRepository
{
}

internal interface ITestRepository : IEventRepository<TestAggregate, AggregateId>
{
}

public record TestCommandNoHandler : DomainCommand
{
}

public record TestCommand : DomainCommand
{
}

