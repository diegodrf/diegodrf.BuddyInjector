namespace diegodrf.BuddyInjector.Tests.Utils.ExampleClass;

public class DisposableClass: IDisposable
{
    public static bool Disposed { get; private set; }

    public void Dispose()
    {
        Disposed = true;
    }
}