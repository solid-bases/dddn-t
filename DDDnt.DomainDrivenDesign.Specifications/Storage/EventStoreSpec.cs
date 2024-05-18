using DDDnt.DomainDrivenDesign.Specifications.Storage.Components;

namespace DDDnt.DomainDrivenDesign.Specifications.Storage
{
    public class EventStoreSpec(EventStoreSteps steps) : SpecificationsWithSteps<IHaveEventStoreSteps, EventStoreSteps, EventStoreContext>(steps)
    {
        [Fact]
        public async Task Should_allow_to_get_by_aggregate_id()
        {
            _steps.Given_the_TestAggregate_events_in_file();
            _steps.Given_the_store();
            await _steps.When_ReadAggregate_is_called("1");
            _steps.Then_the_events_list_is_returned();
        }

        [Fact]
        public void ReadAggregate_Should_throw_if_aggregate_directory_does_not_exists()
        {
            _steps.Given_the_aggregate_directory_not_existing();
            _steps.Given_the_store();
            _steps.Then_the_exception_is_thrown<DirectoryNotFoundException>(_steps.When_ReadAggregate_is_called);
        }

        [Fact]
        public async Task Should_allow_to_commit()
        {
            _steps.Given_the_aggregate_directory_existing();
            _steps.Given_the_store();
            _steps.Given_the_Aggregate_with_id("Commit");
            await _steps.When_commit_is_called();
            _steps.Then_the_aggregate_is_persisted_in_file();
        }

        [Fact]
        public async Task Commit_Should_add_folder_if_not_exists()
        {
            _steps.Given_the_store();
            _steps.Given_the_Aggregate_with_id("Commit");
            await _steps.When_commit_is_called();
            _steps.Then_the_aggregate_is_persisted_in_file();
        }

        [Fact]
        public async Task Should_allow_to_create_a_snapshot()
        {
            _steps.Given_the_TestAggregate_events_in_file();
            _steps.Given_the_store();
            _steps.Given_the_Aggregate_with_id("Commit");
            await _steps.When_snapshot_is_called();
            _steps.Then_the_snapshot_is_persisted();
        }

        [Fact]
        public async Task ReadAggregate_Should_stop_reading_at_snapshot_if_present()
        {
            _steps.Given_the_TestAggregate_events_with_snapshot();
            _steps.Given_the_store();
            await _steps.When_ReadAggregate_is_called("WithSnapshot");
            _steps.Then_events_are_read_up_to_snapshot();
        }
    }
}
