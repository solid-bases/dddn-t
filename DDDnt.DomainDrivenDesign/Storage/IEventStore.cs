using DDDnt.DomainDrivenDesign.Aggregate;
using DDDnt.DomainDrivenDesign.ValueObjects;

using Newtonsoft.Json.Linq;

namespace DDDnt.DomainDrivenDesign.Storage;

public interface IEventStore
{
    Task<JArray> ReadAggregate<T>(CorrelationId correlationId, AggregateId aggregateId, CancellationToken cancellationToken = default) where T : AggregateRoot<AggregateId>;
    Task Commit<T>(CorrelationId correlationId, T aggregate, CancellationToken cancellationToken = default) where T : AggregateRoot<AggregateId>;
    Task Snapshot<T>(CorrelationId correlationId, T aggregate, CancellationToken cancellationToken = default) where T : AggregateRoot<AggregateId>;

}
