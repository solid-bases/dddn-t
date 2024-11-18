using DDDnt.DomainDrivenDesign.Aggregate;
using DDDnt.DomainDrivenDesign.ValueObjects;

using Newtonsoft.Json.Linq;

namespace DDDnt.DomainDrivenDesign.Storage;

public interface IEventStore<T, TId> where TId : AggregateId where T : AggregateRoot<TId>
{
    Task<JArray> ReadAggregate(CorrelationId correlationId, TId aggregateId, CancellationToken cancellationToken = default);
    Task Commit(CorrelationId correlationId, T aggregate, CancellationToken cancellationToken = default);
    Task Snapshot(CorrelationId correlationId, T aggregate, CancellationToken cancellationToken = default);

}
