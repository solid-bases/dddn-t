using DDDnt.DomainDrivenDesign.Specifications.Persistency.Components;

namespace DDDnt.DomainDrivenDesign.Specifications.Persistency;


public class EventRepositorySpec : SpecificationsWithSteps<IHaveEventRepositorySteps, EventRepositorySteps, EventRepositoryContext>
{
    public EventRepositorySpec(EventRepositorySteps steps) : base(steps)
    {
        void Background()
        {
            _steps.Given_the_store_mock();
            _steps.Given_the_repository();
        }
        steps.InitializeContext(Background);
    }

    [Fact]
    public async Task Should_allow_to_get_by_aggregate_id()
    {
        _steps.Given_the_TestAggregate_events();
        await _steps.When_get_by_aggregate_id_is_called("1");
        _steps.Then_the_aggregate_is_initialized();
    }

    [Fact]
    public async Task Should_allow_to_commit()
    {
        _steps.Given_the_Commit_store_mock();
        _steps.Given_the_Aggregate_with_id("Commit");
        await _steps.When_commit_is_called();
        _steps.Then_the_aggregate_is_persisted();
    }

    [Fact]
    public async Task Should_allow_to_create_a_snapshot()
    {
        _steps.Given_the_Snapshot_store_mock();
        _steps.Given_the_TestAggregate_events();
        _steps.Given_the_Aggregate_with_id("Snapshot");
        await _steps.When_snapshot_is_called();
        _steps.Then_the_snapshot_is_created();
    }

}
