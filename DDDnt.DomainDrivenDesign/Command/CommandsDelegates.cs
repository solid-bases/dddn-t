namespace DDDnt.DomainDrivenDesign.Command;

public delegate void ExecuteDelegate(ICommand command);

public class CommandsDelegates : Dictionary<Type, ExecuteDelegate>
{
}
