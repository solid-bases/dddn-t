using DDDnt.DomainDrivenDesign.Aggregate;
using DDDnt.DomainDrivenDesign.EventPublisher;

namespace DDDnt.DomainDrivenDesign.Specifications.CommandsHandler.Components;

internal class TestPublisher : ITestPublisher
{
    public TestPublisher()
    {
    }

    public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
    {
        throw new NotImplementedException();
    }
}

internal interface ITestPublisher : IPublisher
{
}
