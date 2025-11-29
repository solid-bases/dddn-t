using DDDnt.DomainDrivenDesign.EventPublisher;
using DDDnt.DomainDrivenDesign.Persistency;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DDDnt.DomainDrivenDesign.Command;

[Obsolete("Use CommandsReceiver instead")]
public abstract class CommandsHandler(IServiceScopeFactory scopeFactory) : CommandsReceiver<CommandsHandler>(scopeFactory), ICommandHandler
{
}

public abstract class CommandsReceiver<TLogger> : ICommandReceiver
{
    public abstract required CommandsDelegates Delegates { get; init; }
    public abstract RepositoryCollection RepositoriesTypes { get; }
    public virtual PublisherCollection? PublishersTypes { get; } = [];
    protected readonly IServiceProvider _serviceProvider;

    private ILogger<TLogger> Logger { get; }

    private readonly ICollection<IRepository> _repositories = [];
    private readonly ICollection<IPublisher> _publishers = [];

    protected CommandsReceiver(IServiceScopeFactory scopeFactory)
    {
        var scope = scopeFactory.CreateScope();
        _serviceProvider = scope.ServiceProvider;
        Logger = _serviceProvider.GetRequiredService<ILogger<TLogger>>();
        InjectRepositories();
        InjectPublishers();
    }

    private void InjectRepositories()
    {
        foreach (var key in RepositoriesTypes)
        {
            _repositories.Add((IRepository)_serviceProvider.GetRequiredService(key));
        }
    }

    private void InjectPublishers()
    {
        foreach (var key in PublishersTypes ?? [])
        {
            _publishers.Add((IPublisher)_serviceProvider.GetRequiredService(key));
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
            TryExecute(command, execute).Wait();
        }
        else
        {
            Logger.LogError("No handler found for command: {command}", command);
        }
    }

    private async Task TryExecute(ICommand command, ExecuteDelegate execute)
    {
        try
        {
            await execute(command);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error executing command: {command}", command);
            throw;
        }

    }
}
