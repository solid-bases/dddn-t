using Xunit;

namespace DDDnt.TestBase;

/// <summary>
/// This is a base class for all the specifications that need to use steps.<br />
/// As this implements <see cref="IClassFixture{TFixture}"/>, the context will be disposed at each
/// test execution.<br />
/// See <a href="https://xunit.net/docs/shared-context#class-fixture">xUnit.net Class Fixtures</a>
/// </summary>
/// <typeparam name="Steps">
/// Interface containing the steps methods, with implementation,
/// in order to use allow <a href="https://en.wikipedia.org/wiki/Composition_over_inheritance">composition over inheritance</a>.
/// </typeparam>
/// <typeparam name="Context">
/// The context that will be used to store the data of the tests.
/// </typeparam>
/// <typeparam name="StepsImplementation">
/// The implementation of the container that will be used to inject the steps.<br />
/// It is disposable in order to be used as a fixture and it reset the context at each usage.
/// </typeparam>
public abstract class SpecificationsWithSteps<Steps, StepsImplementation, Context> : IClassFixture<StepsImplementation>
    where StepsImplementation : class, IDisposable, Steps, new()
    where Steps : class, IHaveStepsWithContext<Context>
    where Context : class, new()
{
    protected Steps _steps;
    protected SpecificationsWithSteps(StepsImplementation steps)
    {
        _steps = steps;
    }
}
