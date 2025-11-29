using DDDnt.DomainDrivenDesign.Specifications.CommandsHandler.Components;

using Microsoft.Extensions.Logging;

namespace DDDnt.DomainDrivenDesign.Specifications.CommandsHandler;

public class CommandsHandlerSpec : SpecificationsWithSteps<IHaveCommandsReceiverSteps, CommandsHandlerSteps, CommandsHandlerContext>
{
    public CommandsHandlerSpec(CommandsHandlerSteps steps) : base(steps)
    {
        void Background()
        {
            _steps.Given_the_logger_mock();
            _steps.Given_the_get_required_service_mock();
            _steps.Given_the_ServiceProvider_mock();
            _steps.Given_the_TestCommandHandler();
        }
        steps.InitializeContext(Background);
    }

    [Fact]
    public void Handle_should_log_command_received()
    {
        _steps.Given_the_TestCommand();
        _steps.Given_the_log_mocked(LogLevel.Information);
        _steps.When_Handle_is_called();
        _steps.Then_command_received_should_be_logged(LogLevel.Information);
    }

    [Fact]
    public void Handle_should_call_Execute_method()
    {
        _steps.Given_the_TestCommand();
        _steps.Given_the_log_mocked(LogLevel.Information);
        _steps.When_Handle_is_called();
        _steps.Then_Execute_is_called();
    }

    [Fact]
    public void Handle_should_log_exception_when_delegate_throw_exception()
    {
        _steps.Given_the_TestCommandWithException();
        _steps.Given_the_log_mocked(LogLevel.Information);
        _steps.Given_the_log_mocked(LogLevel.Error);
        _steps.Given_an_execution_exception();
        _steps.When_Handle_is_called();
        _steps.Then_TestCommandHandler_contains_two_delegates();
        _steps.Then_command_received_should_be_logged(LogLevel.Error);
    }

    [Fact]
    public void Handle_should_log_exception_when_no_delegate_is_defined()
    {
        _steps.Given_the_TestCommandNoHandler();
        _steps.Given_the_log_mocked(LogLevel.Information);
        _steps.Given_the_log_mocked(LogLevel.Error);
        _steps.When_Handle_is_called();
        _steps.Then_command_received_should_be_logged(LogLevel.Error);
    }

    [Fact]
    public void Should_allow_to_inject_repositories()
    {
        _steps.Then_the_repositories_are_injected();
    }

    [Fact]
    public void Should_allow_to_inject_publishers()
    {
        _steps.Then_the_publishers_are_injected();
    }

}

