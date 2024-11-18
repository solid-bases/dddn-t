using DDDnt.DomainDrivenDesign.Storage;
using DDDnt.DomainDrivenDesign.ValueObjects;

using Moq;

using Newtonsoft.Json.Linq;

namespace DDDnt.DomainDrivenDesign.Specifications.Persistency.Components;

public interface IHaveEventRepositorySteps : IHaveStepsWithContext<EventRepositoryContext>
{
    void Given_the_store_mock()
    {
        Context.StoreMock = new Mock<IEventStore<TestAggregate, AggregateId>>();
    }

    void Given_the_TestAggregate_events()
    {
        Context.Events = new JArray(
            new JObject { { "EventName", typeof(TestCreateEvent).FullName }, { "Id", "1" }, { "Description", "Test" } },
            new JObject { { "EventName", typeof(TestUpdateEvent).FullName }, { "Id", "1" }, { "Description", "Test" }, { "OnlyUpdate", 1 } }
        );
        Context.StoreMock!
            .Setup(s => s.ReadAggregate(It.IsAny<CorrelationId>(), It.IsAny<AggregateId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Context.Events);
    }

    void Given_the_repository()
    {
        Context.Repository = new TestRepository(Context.StoreMock!.Object);
    }

    async Task When_get_by_aggregate_id_is_called(string id)
    {
        Context.Aggregate = await Context.Repository!.GetByAggregateId(new(), new(id));
    }

    void Then_the_aggregate_is_initialized()
    {
        Context.Aggregate!.Should().NotBeNull();
        Context.Aggregate!.Id!.Value.Should().Be("1");
        Context.Aggregate!.Description!.Value.Should().Be("Test");
        Context.Aggregate!.OnlyUpdate!.Value.Should().Be(1);
    }

    void Given_the_Aggregate_with_id(string aggregateId)
    {
        Context.Aggregate = new TestAggregate { Id = new(aggregateId), Description = new("Test") };
    }

    void Given_the_Commit_store_mock()
    {
        Context.StoreMock!
            .Setup(s => s.Commit(It.IsAny<CorrelationId>(), It.IsAny<TestAggregate>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
    }

    Task When_commit_is_called()
    {
        return Context.Repository!.Commit(new(), Context.Aggregate!);
    }

    void Then_the_aggregate_is_persisted()
    {
        Context.StoreMock!.Verify(s => s.Commit(It.IsAny<CorrelationId>(), Context.Aggregate!, It.IsAny<CancellationToken>()), Times.Once);
    }

    void Given_the_Snapshot_store_mock()
    {
        Context.StoreMock!
            .Setup(s => s.Snapshot(It.IsAny<CorrelationId>(), It.IsAny<TestAggregate>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
    }

    Task When_snapshot_is_called()
    {
        return Context.Repository!.Snapshot(new(), Context.Aggregate!);
    }

    void Then_the_snapshot_is_created()
    {
        Context.StoreMock!.Verify(s => s.Snapshot(It.IsAny<CorrelationId>(), Context.Aggregate!, It.IsAny<CancellationToken>()), Times.Once);
    }

}
