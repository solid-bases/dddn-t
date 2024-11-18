using System.IO.Abstractions;

using DDDnt.DomainDrivenDesign.Specifications.Persistency.Components;
using DDDnt.DomainDrivenDesign.Storage;
using DDDnt.DomainDrivenDesign.ValueObjects;

namespace DDDnt.DomainDrivenDesign.Specifications.Persistency;

public class TestStore(IFileSystem fileSystem) : EventStore<TestAggregate, AggregateId>(fileSystem)
{
}
