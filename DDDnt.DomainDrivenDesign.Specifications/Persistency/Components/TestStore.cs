using System.IO.Abstractions;

using DDDnt.DomainDrivenDesign.Storage;

namespace DDDnt.DomainDrivenDesign.Specifications.Persistency;

public class TestStore(IFileSystem fileSystem) : EventStore(fileSystem)
{
}
