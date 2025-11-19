namespace DDDnt.DomainDrivenDesign.Command;

public delegate Task ExecuteDelegate(ICommand command);

public class CommandsDelegates : Dictionary<Type, ExecuteDelegate>
{
}
