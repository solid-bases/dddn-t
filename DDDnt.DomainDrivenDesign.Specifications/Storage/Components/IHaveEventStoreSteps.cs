using System.IO.Abstractions.TestingHelpers;

using DDDnt.DomainDrivenDesign.Aggregate;
using DDDnt.DomainDrivenDesign.Persistency;
using DDDnt.DomainDrivenDesign.Specifications.Persistency.Components;
using DDDnt.DomainDrivenDesign.ValueObjects;

using Newtonsoft.Json;

namespace DDDnt.DomainDrivenDesign.Specifications.Storage.Components;

public interface IHaveEventStoreSteps : IHaveStepsWithContext<EventStoreContext>
{
    void Given_the_TestAggregate_events_in_file()
    {
        IList<IDomainEvent> events = [
            new TestCreateEvent("1", "Test"),
            new TestUpdateEvent("1", "Test", 1)
        ];
        Context.FileSystemMock = new MockFileSystem(new Dictionary<string, MockFileData> {
            { Path.Combine("/", $"{typeof(TestAggregate).Name}/1.json"), new(string.Join(Environment.NewLine, events.Select(JsonConvert.SerializeObject))) }
        });
    }

    void Given_the_store()
    {
        Context.Store = new TestStore(Context.FileSystemMock);
    }

    async Task When_ReadAggregate_is_called(string aggregateId)
    {
        Context.Events = await Context.Store!.ReadAggregate(new(), new(aggregateId));
    }

    void Then_the_events_list_is_returned()
    {
        Assert.NotNull(Context.Events);
        Assert.Equal(2, Context.Events!.Count());
        Assert.Equal(typeof(TestCreateEvent).FullName, Context.Events![0]!["EventName"]!.ToString());
        Assert.Equal(typeof(TestUpdateEvent).FullName, Context.Events![1]!["EventName"]!.ToString());
    }

    void Given_the_aggregate_directory_not_existing()
    {
        Context.FileSystemMock = new MockFileSystem();
    }

    void Then_the_exception_is_thrown<T>(Func<string, Task> when_ReadAggregate_is_called) where T : Exception
    {
        var act = () => when_ReadAggregate_is_called("1").Wait();
        Assert.Throws<T>(act);
    }

    void Given_the_Aggregate_with_id(string aggregateId)
    {
        Context.Aggregate = new TestAggregate { Id = new(aggregateId), Description = new("Test") };
    }

    Task When_commit_is_called()
    {
        return Context.Store!.Commit(new(), Context.Aggregate!);
    }

    void Then_the_aggregate_is_persisted_in_file()
    {
        Assert.True(Context.FileSystemMock!.FileExists($@"/{typeof(TestAggregate).Name}/{Context.Aggregate!.Id!.Value}.json"));
    }

    void Given_the_aggregate_directory_existing()
    {
        Context.FileSystemMock = new MockFileSystem(new Dictionary<string, MockFileData> {
            { $@"/{typeof(TestAggregate).Name}", new MockDirectoryData() }
        });
    }

    Task When_snapshot_is_called()
    {
        return Context.Store!.Snapshot(new(), Context.Aggregate!);
    }

    void Then_the_snapshot_is_persisted()
    {
        Assert.True(Context.FileSystemMock!.FileExists($@"/{typeof(TestAggregate).Name}/{Context.Aggregate!.Id!.Value}.json"));
        var fileContent = Context.FileSystemMock!.File.ReadAllText($@"/{typeof(TestAggregate).Name}/{Context.Aggregate!.Id!.Value}.json");
        Assert.Contains($"\"EventName\":\"Snapshot-{typeof(TestAggregate).Name}\"", fileContent);
    }

    void Given_the_TestAggregate_events_with_snapshot()
    {
        IList<IDomainEvent> events = [
            new TestCreateEvent("WithSnapshot", "Test"),
            new TestUpdateEvent("WithSnapshot", "Test", 1),
            new Snapshot<TestAggregate, AggregateId>(new(), new("No User"), new TestAggregate { Id = new("WithSnapshot"), Description = new("Test-snapshot") }),
            new TestUpdateEvent("WithSnapshot", null, 2),
        ];
        Context.FileSystemMock = new MockFileSystem(new Dictionary<string, MockFileData> {
            { $@"/{typeof(TestAggregate).Name}/WithSnapshot.json", new(string.Join(Environment.NewLine, events.Select(JsonConvert.SerializeObject))) }
        });
    }

    void Then_events_are_read_up_to_snapshot()
    {
        Assert.NotNull(Context.Events);
        Assert.Equal(2, Context.Events!.Count());
        Assert.Equal($"Snapshot-{typeof(TestAggregate).Name}", Context.Events![0]!["EventName"]!.ToString());
        Assert.Equal(typeof(TestUpdateEvent).FullName, Context.Events![1]!["EventName"]!.ToString());
    }
}
