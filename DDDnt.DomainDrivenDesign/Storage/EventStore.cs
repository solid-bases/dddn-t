using System.IO.Abstractions;

using DDDnt.DomainDrivenDesign.Aggregate;
using DDDnt.DomainDrivenDesign.Persistency;
using DDDnt.DomainDrivenDesign.ValueObjects;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DDDnt.DomainDrivenDesign.Storage;

public class EventStore(IFileSystem fileSystem) : IEventStore
{
    protected IFileSystem _fileSystem = fileSystem;

    public async Task<JArray> ReadAggregate<T>(CorrelationId correlationId, AggregateId aggregateId, CancellationToken cancellationToken = default) where T : AggregateRoot<AggregateId>
    {
        if (!_fileSystem.Directory.Exists(typeof(T).Name))
        {
            throw new DirectoryNotFoundException(typeof(T).Name);
        }

        var filename = $"{typeof(T).Name}/{aggregateId.Value}.json";
        var fileLines = (await _fileSystem.File.ReadAllLinesAsync(filename, cancellationToken)).ToList();
        fileLines = TailFileLinesFromLastSnapshot<T>(fileLines);
        var fileContent = string.Join(",", fileLines);
        var events = JArray.Parse($"[{fileContent}]");
        return events;
    }

    private static List<string> TailFileLinesFromLastSnapshot<T>(List<string> fileLines) where T : AggregateRoot<AggregateId>
    {
        var index = fileLines.FindLastIndex(l => l.Contains($"Snapshot-{typeof(T).Name}"));
        if (index >= 0)
        {
            fileLines = fileLines.GetRange(index, fileLines.Count - index);
        }

        return fileLines;
    }


    public async Task Commit<T>(CorrelationId correlationId, T aggregate, CancellationToken cancellationToken = default) where T : AggregateRoot<AggregateId>
    {
        CreateAggregateFolderIfNotExists<T>();
        var (fileContent, filename) = GenerateFileContentFromUncommittedEvents(aggregate);

        if (_fileSystem.File.Exists(filename))
        {
            await _fileSystem.File.AppendAllLinesAsync(filename, fileContent, cancellationToken);
        }
        else
        {
            await _fileSystem.File.WriteAllLinesAsync(filename, fileContent, cancellationToken);
        }
    }

    private static (string[] fileContent, string filename) GenerateFileContentFromUncommittedEvents<T>(T aggregate) where T : AggregateRoot<AggregateId>
    {
        var events = aggregate.ClearUncommittedEvents();
        var fileContent = events.Select(JsonConvert.SerializeObject).ToArray();
        var filename = $"{typeof(T).Name}/{aggregate.Id!.Value}.json";
        return (fileContent, filename);
    }

    private void CreateAggregateFolderIfNotExists<T>() where T : AggregateRoot<AggregateId>
    {
        if (!_fileSystem.Directory.Exists(typeof(T).Name))
        {
            _fileSystem.Directory.CreateDirectory(typeof(T).Name);
        }
    }


    public Task Snapshot<T>(CorrelationId correlationId, T aggregate, CancellationToken cancellationToken = default) where T : AggregateRoot<AggregateId>
    {
        if (aggregate.Id is null)
        {
            throw new ArgumentNullException(nameof(aggregate.Id));
        }
        CreateAggregateFolderIfNotExists<T>();
        var (fileContent, filename) = GenerateFileContentFromUncommittedEvents(aggregate);
        fileContent = [.. fileContent, JsonConvert.SerializeObject(new Snapshot<T>(correlationId, new("System"), aggregate))];
        return _fileSystem.File.WriteAllLinesAsync(filename, fileContent, cancellationToken);
    }
}
