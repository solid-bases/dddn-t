using DDDnt.DomainDrivenDesign.Aggregate;

using Microsoft.Extensions.Logging;

using Moq;

namespace DDDnt.DomainDrivenDesign.Specifications.Publisher.Components;

public class PublisherContext
{
    public Mock<ILogger<TestPublisher>>? LoggerMock { get; internal set; }

    public IAggregateEvent? AggregateEvent { get; internal set; }

    internal TestPublisher? Publisher { get; set; }
}
