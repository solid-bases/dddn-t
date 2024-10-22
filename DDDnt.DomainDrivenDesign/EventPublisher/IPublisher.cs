namespace DDDnt.DomainDrivenDesign.EventPublisher;

public interface IPublisher
{
    void Publish<TEvent>(TEvent @event) where TEvent : IIntegrationEvent;
}
