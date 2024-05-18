using System.IO.Abstractions.TestingHelpers;

using Newtonsoft.Json.Linq;

using DDDnt.DomainDrivenDesign.Specifications.Persistency.Components;
using DDDnt.DomainDrivenDesign.Storage;
namespace DDDnt.DomainDrivenDesign.Specifications.Storage.Components;

public class EventStoreContext
{
    public MockFileSystem FileSystemMock { get; internal set; } = new MockFileSystem();
    public IEventStore? Store { get; internal set; }
    public JToken? Events { get; internal set; }
    public TestAggregate? Aggregate { get; internal set; }
}
