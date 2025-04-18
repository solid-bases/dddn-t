using System.IO.Abstractions;

using DDDnt.DomainDrivenDesign.Storage;
using DDDnt.DomainDrivenDesign.ValueObjects;

namespace DDDnt.DomainDrivenDesign.Specifications.Persistency.Components;

public class TestStore(IFileSystem fileSystem) : EventStore<TestAggregate, AggregateId>(fileSystem)
{
}
