using Microsoft.Extensions.Logging;

namespace DDDnt.DomainDrivenDesign.EventPublisher;

public abstract class Publisher(ILogger<Publisher> logger) : IPublisher
{
    private ILogger<Publisher> _logger { get; } = logger;
    public required abstract EventsDelegates Delegates { get; set; }

    public virtual void Publish<TEvent>(TEvent @event) where TEvent : IIntegrationEvent
    {
        _logger.LogInformation("Event received: {event}", @event);
        if (Delegates.TryGetValue(@event.GetType(), out var execute))
        {
            TryExecute(@event, execute);
        }
        else
        {
            _logger.LogError("No handler found for event: {@event}", @event);
        }
    }

    private void TryExecute(IIntegrationEvent @event, ExecuteDelegate execute)
    {
        try
        {
            execute(@event);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing event: {@event}", @event);
        }

    }
}
