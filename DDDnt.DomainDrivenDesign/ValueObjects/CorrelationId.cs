namespace DDDnt.DomainDrivenDesign.ValueObjects;

public record CorrelationId(Guid Value) : ValueObject
{
    public CorrelationId() : this(Guid.NewGuid())
    {

    }
}
