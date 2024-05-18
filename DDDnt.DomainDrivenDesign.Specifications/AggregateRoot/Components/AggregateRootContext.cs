using DDDnt.DomainDrivenDesign.Aggregate;

namespace DDDnt.DomainDrivenDesign.Specifications.AggregateRoot.Components;

public class AggregateRootContext
{
    public TestAggregateRoot? AggregateRoot { get; set; }
    public IDomainEvent? Event { get; set; }
    public ICollection<IDomainEvent>? UncommittedEvents { get; internal set; }
}
