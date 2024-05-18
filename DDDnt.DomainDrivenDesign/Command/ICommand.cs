using DDDnt.DomainDrivenDesign.ValueObjects;

namespace DDDnt.DomainDrivenDesign.Command;

public interface ICommand
{
    CorrelationId CorrelationId { get; init; }
    User User { get; init; }
}
