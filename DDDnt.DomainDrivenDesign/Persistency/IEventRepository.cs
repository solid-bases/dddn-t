using DDDnt.DomainDrivenDesign.Aggregate;
using DDDnt.DomainDrivenDesign.ValueObjects;

namespace DDDnt.DomainDrivenDesign.Persistency;

public interface IEventRepository : IRepository
{
    Task<T> GetByAggregateId<T>(CorrelationId correlationId, AggregateId id, CancellationToken cancellationToken = default) where T : AggregateRoot<AggregateId>, new();
    Task Commit<T>(CorrelationId correlationId, T aggregate, CancellationToken cancellationToken = default) where T : AggregateRoot<AggregateId>;
    Task Snapshot<T>(CorrelationId correlationId, T aggregate, CancellationToken cancellationToken = default) where T : AggregateRoot<AggregateId>;
}
