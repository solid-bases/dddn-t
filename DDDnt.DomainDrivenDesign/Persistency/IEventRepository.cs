using DDDnt.DomainDrivenDesign.Aggregate;
using DDDnt.DomainDrivenDesign.ValueObjects;

namespace DDDnt.DomainDrivenDesign.Persistency;

public interface IEventRepository<T, TId> : IRepository where TId : AggregateId where T : AggregateRoot<TId>, new()
{
    Task<T> GetByAggregateId(CorrelationId correlationId, TId id, CancellationToken cancellationToken = default);
    Task Commit(CorrelationId correlationId, T aggregate, CancellationToken cancellationToken = default);
    Task Snapshot(CorrelationId correlationId, T aggregate, CancellationToken cancellationToken = default);
}
