namespace DDDnt.DomainDrivenDesign.Command;

public interface ICommandHandler
{
    void Handle<TCommand>(TCommand command) where TCommand : ICommand;
}
