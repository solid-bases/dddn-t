namespace DDDnt.TestBase;

public delegate void BackgroundDelegate();

public abstract class StepsContainer<T> : IDisposable, IHaveStepsWithContext<T>
    where T : class, new()
{
    public T Context { get; private set; }

    public BackgroundDelegate? Background { get; private set; }

    protected StepsContainer()
    {
        Context = new();
    }

    public virtual void InitializeContext(BackgroundDelegate? background = default)
    {
        Context = new();
        Background = background;
        Background?.Invoke();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Background = null;
            Context = new();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
