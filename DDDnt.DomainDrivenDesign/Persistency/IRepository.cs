namespace DDDnt.DomainDrivenDesign.Persistency;

public interface IRepository
{
}

public interface IHaveGetById : IRepository
{
    Task<T> GetById<T>(string id) where T : class;
}

public interface IHaveCreate : IRepository
{
    Task Create<T>(T entity) where T : class;
}

public interface IHaveUpdate : IRepository
{
    Task Update<T>(T entity) where T : class;
}

public interface IHaveDelete : IRepository
{
    Task Delete<T>(T entity) where T : class;
}
