namespace diegodrf.BuddyInjector.Tests.Utils.ExampleClass;

public class Bar : IBar
{
    public IFoo Foo { get; }

    public Bar(IFoo foo)
    {
        Foo = foo;
    }

    public string SayGoodBye()
    {
        return "Goodbye Bar!";
    }

    
}