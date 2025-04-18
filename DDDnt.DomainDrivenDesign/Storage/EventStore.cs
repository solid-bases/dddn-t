using System.IO.Abstractions;

using DDDnt.DomainDrivenDesign.Aggregate;
using DDDnt.DomainDrivenDesign.Persistency;
using DDDnt.DomainDrivenDesign.ValueObjects;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DDDnt.DomainDrivenDesign.Storage;

public class EventStore<T, TId>(IFileSystem fileSystem) : IEventStore<T, TId> where TId : AggregateId where T : AggregateRoot<TId>
{
    protected IFileSystem _fileSystem = fileSystem;

    public async Task<JArray> ReadAggregate(CorrelationId correlationId, TId aggregateId, CancellationToken cancellationToken = default)
    {
        if (!_fileSystem.Directory.Exists(typeof(T).Name))
        {
            throw new DirectoryNotFoundException(typeof(T).Name);
        }

        var filename = $"{typeof(T).Name}/{aggregateId.Value}.json";
        var fileLines = (await _fileSystem.File.ReadAllLinesAsync(filename, cancellationToken)).ToList();
        fileLines = TailFileLinesFromLastSnapshot(fileLines);
        var fileContent = string.Join(",", fileLines);
        var events = JArray.Parse($"[{fileContent}]");
        return events;
    }

    private static List<string> TailFileLinesFromLastSnapshot(List<string> fileLines)
    {
        var index = fileLines.FindLastIndex(l => l.Contains($"Snapshot-{typeof(T).Name}"));
        if (index >= 0)
        {
            fileLines = fileLines.GetRange(index, fileLines.Count - index);
        }

        return fileLines;
    }


    public async Task Commit(CorrelationId correlationId, T aggregate, CancellationToken cancellationToken = default)
    {
        CreateAggregateFolderIfNotExists();
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

    private static (string[] fileContent, string filename) GenerateFileContentFromUncommittedEvents(T aggregate)
    {
        var events = aggregate.ClearUncommittedEvents();
        var fileContent = events.Select(JsonConvert.SerializeObject).ToArray();
        var filename = $"{typeof(T).Name}/{aggregate.Id!.Value}.json";
        return (fileContent, filename);
    }

    private void CreateAggregateFolderIfNotExists()
    {
        if (!_fileSystem.Directory.Exists(typeof(T).Name))
        {
            _fileSystem.Directory.CreateDirectory(typeof(T).Name);
        }
    }


    public Task Snapshot(CorrelationId correlationId, T aggregate, CancellationToken cancellationToken = default)
    {
        if (aggregate.Id is null)
        {
            throw new ArgumentNullException(nameof(aggregate), "Aggregate ID cannot be null when taking a snapshot.");
        }
        CreateAggregateFolderIfNotExists();
        var (fileContent, filename) = GenerateFileContentFromUncommittedEvents(aggregate);
        fileContent = [.. fileContent, JsonConvert.SerializeObject(new Snapshot<T, TId>(correlationId, new("System"), aggregate))];
        return _fileSystem.File.WriteAllLinesAsync(filename, fileContent, cancellationToken);
    }

}
