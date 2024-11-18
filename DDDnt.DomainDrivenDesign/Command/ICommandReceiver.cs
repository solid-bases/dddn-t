namespace DDDnt.DomainDrivenDesign.Command;

[Obsolete("Use ICommandReceiver instead")]
public interface ICommandHandler : ICommandReceiver { }

public interface ICommandReceiver
{
    void Handle<TCommand>(TCommand command) where TCommand : ICommand;
}
