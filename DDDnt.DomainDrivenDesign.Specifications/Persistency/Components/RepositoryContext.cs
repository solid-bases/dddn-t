
using DDDnt.DomainDrivenDesign.Persistency;
using DDDnt.DomainDrivenDesign.Storage;

using Moq;

using Newtonsoft.Json.Linq;

namespace DDDnt.DomainDrivenDesign.Specifications.Persistency.Components;

public class EventRepositoryContext
{
    public IEventRepository? Repository { get; internal set; }
    public TestAggregate? Aggregate { get; internal set; }
    public JArray? Events { get; internal set; }
    public Mock<IEventStore>? StoreMock { get; internal set; }
}
