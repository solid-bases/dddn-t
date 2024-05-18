using DDDnt.DomainDrivenDesign.Aggregate;
using DDDnt.DomainDrivenDesign.Persistency;
using DDDnt.DomainDrivenDesign.Storage;
using DDDnt.DomainDrivenDesign.ValueObjects;

namespace DDDnt.DomainDrivenDesign.Specifications.Persistency.Components;

internal record TestCreateEvent(CorrelationId CorrelationId, User User, string Id, string Description) : IDomainEvent
{

    public string EventName => GetType().FullName ?? GetType().Name;

    public TestCreateEvent() : this(new(), new("No User"), string.Empty, string.Empty)
    {
    }

    public TestCreateEvent(string id, string description) : this(new(), new("No User"), id, description)
    {
    }
}

internal record TestUpdateEvent(CorrelationId CorrelationId, User User, string Id, string? Description = null, int? OnlyUpdate = null) : IDomainEvent
{
    public string EventName => GetType().FullName ?? GetType().Name;

    public TestUpdateEvent() : this(new(), new("No User"), string.Empty, string.Empty, 0)
    {
    }

    public TestUpdateEvent(string id, string? description, int? onlyUpdate) : this(new(), new("No User"), id, description, onlyUpdate)
    {
    }
}

public record TestOnlyUpdate(int Value);

public class TestAggregate : Aggregate.AggregateRoot<AggregateId>
{
    public TestDescription? Description { get; set; }
    public TestOnlyUpdate? OnlyUpdate { get; set; }

    public override DomainEventsDelegates DomainEventsCollection { get; }
    public TestAggregate() : base()
    {
        DomainEventsCollection = new() {
        { typeof(TestCreateEvent), @event => Apply((TestCreateEvent)@event) },
        { typeof(TestUpdateEvent), @event => Apply((TestUpdateEvent)@event) },
        { typeof(Snapshot<TestAggregate>), @event => Apply((Snapshot<TestAggregate>)@event) },
    };
    }

    public int SnapshotCallCount { get; set; } = 0;
    public int UpdateCallCount { get; set; } = 0;
    public int CreateCallCount { get; set; } = 0;
    public override AggregateId? Id { get; set; }


    private void Apply(Snapshot<TestAggregate> @event)
    {
        Id = @event.AggregateData.Id;
        Description = @event.AggregateData.Description;
        OnlyUpdate = @event.AggregateData.OnlyUpdate;
        SnapshotCallCount++;
    }

    private void Apply(TestUpdateEvent @event)
    {
        Id = new(@event.Id);
        if (@event.Description is not null) Description = new(@event.Description);
        if (@event.OnlyUpdate is not null) OnlyUpdate = new(@event.OnlyUpdate.Value);
        UpdateCallCount++;
    }

    private void Apply(TestCreateEvent @event)
    {
        Id = new(@event.Id);
        Description = new(@event.Description);
        CreateCallCount++;
    }
}

public record TestDescription(string Value);

public class TestRepository(IEventStore store) : EventRepository(store)
{
}
