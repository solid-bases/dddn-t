using DDDnt.DomainDrivenDesign.Aggregate;

using Microsoft.Extensions.Logging;

namespace DDDnt.DomainDrivenDesign.EventPublisher;

public abstract class Publisher(ILogger<Publisher> logger) : IPublisher
{
    private ILogger<Publisher> Logger { get; } = logger;
    public required abstract EventsDelegates Delegates { get; set; }

    public virtual void Publish<TEvent>(TEvent @event) where TEvent : IEvent
    {
        Logger.LogInformation("Event received: {event}", @event);
        if (Delegates.TryGetValue(@event.GetType(), out var execute))
        {
            TryExecute(@event, execute);
        }
        else
        {
            Logger.LogError("No handler found for event: {@event}", @event);
        }
    }

    private void TryExecute(IEvent @event, ExecuteDelegate execute)
    {
        try
        {
            execute(@event);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error executing event: {@event}", @event);
        }

    }
}
