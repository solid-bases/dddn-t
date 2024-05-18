using DDDnt.DomainDrivenDesign.Command;

namespace DDDnt.DomainDrivenDesign.Sender;

public interface ISender
{
    Task Send<T>(T command) where T : ICommand;
}
