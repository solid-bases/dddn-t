using System.IO.Abstractions.TestingHelpers;

using DDDnt.DomainDrivenDesign.Specifications.Persistency.Components;
using DDDnt.DomainDrivenDesign.Storage;
using DDDnt.DomainDrivenDesign.ValueObjects;

using Newtonsoft.Json.Linq;
namespace DDDnt.DomainDrivenDesign.Specifications.Storage.Components;

public class EventStoreContext
{
    public MockFileSystem FileSystemMock { get; internal set; } = new MockFileSystem();
    public IEventStore<TestAggregate, AggregateId>? Store { get; internal set; }
    public JToken? Events { get; internal set; }
    public TestAggregate? Aggregate { get; internal set; }
}
