using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using DDDnt.DomainDrivenDesign.Persistency;

namespace DDDnt.DomainDrivenDesign.Command;

public abstract class CommandsHandler : ICommandHandler
{
    public abstract CommandsDelegates Delegates { get; }
    public abstract RepositoryCollection RepositoriesTypes { get; }

    private ILogger<CommandsHandler> _logger { get; }

    private readonly ICollection<IRepository> _repositories = [];
    protected CommandsHandler(ILogger<CommandsHandler> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;

        InjectRepositories(serviceProvider);
    }

    private void InjectRepositories(IServiceProvider serviceProvider)
    {
        foreach (var key in RepositoriesTypes)
        {
            _repositories.Add((IEventRepository)serviceProvider.GetRequiredService(key));
        }
    }

    public T? GetRepository<T>() where T : IRepository
    {
        return (T?)_repositories.FirstOrDefault(r => r is T);
    }

    public void Handle<TCommand>(TCommand command) where TCommand : ICommand
    {
        _logger.LogInformation("Command received: {command}", command);
        if (Delegates.TryGetValue(command.GetType(), out var execute))
        {
            TryExecute(command, execute);
        }
        else
        {
            _logger.LogError("No handler found for command: {command}", command);
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
            _logger.LogError(ex, "Error executing command: {command}", command);
        }

    }
}
