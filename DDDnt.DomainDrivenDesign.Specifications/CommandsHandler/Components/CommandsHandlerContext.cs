using Microsoft.Extensions.Logging;

using Moq;

using DDDnt.DomainDrivenDesign.Command;

namespace DDDnt.DomainDrivenDesign.Specifications.CommandsHandler.Components;

public class CommandsHandlerContext
{
    public Mock<ILogger<TestCommandsHandler>>? LoggerMock { get; internal set; }
    public ICommand? Command { get; internal set; }
    public Mock<IServiceProvider>? ServiceProviderMock { get; internal set; }
    internal TestCommandsHandler? CommandsHandler { get; set; }
}

