using DDDnt.DomainDrivenDesign.Aggregate;
namespace DDDnt.DomainDrivenDesign.EventPublisher;

public interface IPublisher
{
    void Publish<TEvent>(TEvent @event) where TEvent : IAggregateEvent;
}
