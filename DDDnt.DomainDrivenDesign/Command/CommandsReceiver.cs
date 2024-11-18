using DDDnt.DomainDrivenDesign.EventPublisher;
using DDDnt.DomainDrivenDesign.Persistency;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DDDnt.DomainDrivenDesign.Command;

[Obsolete("Use CommandsReceiver instead")]
public abstract class CommandsHandler(ILogger<CommandsHandler> logger, IServiceProvider serviceProvider) : CommandsReceiver(logger, serviceProvider), ICommandHandler
{
}

public abstract class CommandsReceiver : ICommandReceiver
{
    public abstract required CommandsDelegates Delegates { get; init; }
    public abstract RepositoryCollection RepositoriesTypes { get; }
    public virtual PublisherCollection? PublishersTypes { get; } = [];

    private ILogger<CommandsReceiver> Logger { get; }

    private readonly ICollection<IRepository> _repositories = [];
    private readonly ICollection<IPublisher> _publishers = [];

    protected CommandsReceiver(ILogger<CommandsReceiver> logger, IServiceProvider serviceProvider)
    {
        Logger = logger;

        InjectRepositories(serviceProvider);
        InjectPublishers(serviceProvider);
    }

    private void InjectRepositories(IServiceProvider serviceProvider)
    {
        foreach (var key in RepositoriesTypes)
        {
            _repositories.Add((IRepository)serviceProvider.GetRequiredService(key));
        }
    }

    private void InjectPublishers(IServiceProvider serviceProvider)
    {
        foreach (var key in PublishersTypes ?? [])
        {
            _publishers.Add((IPublisher)serviceProvider.GetRequiredService(key));
        }
    }

    public T? GetRepository<T>() where T : IRepository
    {
        return (T?)_repositories.FirstOrDefault(r => r is T);
    }

    public T? GetPublisher<T>() where T : IPublisher
    {
        return (T?)_publishers.FirstOrDefault(r => r is T);
    }

    public void Handle<TCommand>(TCommand command) where TCommand : ICommand
    {
        Logger.LogInformation("Command received: {command}", command);
        if (Delegates.TryGetValue(command.GetType(), out var execute))
        {
            TryExecute(command, execute);
        }
        else
        {
            Logger.LogError("No handler found for command: {command}", command);
        }
    }

    private void TryExecute(ICommand command, ExecuteDelegate execute)
    {
        try
        {
            execute(command);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error executing command: {command}", command);
        }

    }
}
