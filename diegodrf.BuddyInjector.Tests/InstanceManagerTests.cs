namespace diegodrf.BuddyInjector.Tests;

public class InstanceManagerTests
{
    [Fact]
    public void Should_Create_A_Singleton()
    {

        var instance = new InstanceManager<Foo>(() => new Foo(), true);

        var a = instance.GetInstance();
        var b = instance.GetInstance();
        
        Assert.Equal(a, b);
    }
    
    [Fact]
    public void Should_Create_A_Transient()
    {

        var instance = new InstanceManager<Foo>(() => new Foo(), false);

        var a = instance.GetInstance();
        var b = instance.GetInstance();
        
        Assert.NotEqual(a, b);
    }
}