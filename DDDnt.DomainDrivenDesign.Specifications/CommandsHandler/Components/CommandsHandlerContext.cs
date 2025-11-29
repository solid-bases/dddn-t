using DDDnt.DomainDrivenDesign.Command;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Moq;

namespace DDDnt.DomainDrivenDesign.Specifications.CommandsHandler.Components;

public class CommandsHandlerContext
{
    public Mock<ILogger<TestCommandsReceiver>>? LoggerMock { get; internal set; }
    public ICommand? Command { get; internal set; }
    public Mock<IServiceScopeFactory>? ScopeFactoryMock { get; internal set; }
    public IServiceProvider? ServiceProvider { get; internal set; }
    public IServiceCollection? Services { get; internal set; }
    internal TestCommandsReceiver? CommandsHandler { get; set; }
}

