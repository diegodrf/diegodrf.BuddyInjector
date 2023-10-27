namespace diegodrf.BuddyInjector.Tests;

public class BuddyInjectorTests
{
    [Fact]
    public void Should_Create_A_Singleton()
    {
        var container = new BuddyInjector();
        container.RegisterSingleton(() => new Foo());

        var a = container.GetInstance<Foo>();
        var b = container.GetInstance<Foo>();
        
        Assert.Equal(a, b);
    }
    
    [Fact]
    public void Should_Create_A_Transient()
    {

        var container = new BuddyInjector();
        container.RegisterTransient(() => new Foo());

        var a = container.GetInstance<Foo>();
        var b = container.GetInstance<Foo>();
        
        Assert.NotEqual(a, b);
    }

    [Fact]
    public void Should_Create_An_Object_Based_On_Interface()
    {
        var container = new BuddyInjector();
        container.RegisterSingleton<IFoo>(() => new Foo());

        var value = container.GetInstance<IFoo>();
        
        Assert.IsAssignableFrom<IFoo>(value);
    }
    
    [Fact]
    public void Should_Return_NotRegisteredException_When_Get_An_Object_Not_Registered()
    {
        var container = new BuddyInjector();
        
        Assert.Throws<NotRegisteredException>(() => container.GetInstance<IBar>());
    }
    
    [Fact]
    public void Should_Register_All_Instances_From_Lambda()
    {
        var container = new BuddyInjector()
            .RegisterAll(x =>
        {
            x.RegisterSingleton<IFoo>(() => new Foo());
            x.RegisterSingleton<IBar>(() => new Bar(x.GetInstance<IFoo>()));
        });

        var instance = container.GetInstance<IBar>();

        Assert.IsAssignableFrom<IBar>(instance);
    }
    
    [Fact]
    public void Should_Register_All_Instances_No_Matter_The_Order()
    {
        var container = new BuddyInjector();
        container.RegisterTransient<IBar>(() => new Bar(container.GetInstance<IFoo>()));
        container.RegisterTransient<IFoo>(() => new Foo());

        var instance = container.GetInstance<IBar>();
        var message = instance.Foo.SayHello();
        
        Assert.Equal("Hello Foo!", message);
    }

    [Fact]
    public void Given_More_Than_Once_Register_For_Same_Interface_Should_Use_Only_The_Last_Register()
    {
        // Arrange
        var sut = new BuddyInjector();
        sut.RegisterSingleton<IFoo>(() => new Foo());
        sut.RegisterSingleton<IFoo>(() => new Foo2());
        
        // Act
        var instance = sut.GetInstance<IFoo>();
        
        // Assert
        // Assert
        Assert.Equal("Hello Foo2!", instance.SayHello());
        Assert.IsAssignableFrom<Foo2>(instance);

    }
    
    [Fact]
    public void Given_More_Than_Once_Register_For_Same_Interface_Should_Discard_The_Previous_Register()
    {
        // Arrange
        var sut = new BuddyInjector();
        sut.RegisterSingleton<IFoo>(() => new Foo());
        sut.RegisterSingleton<IFoo>(() => new Foo2());
        
        // Act
        var instance = sut.GetInstance<IFoo>();
        
        // Assert
        Assert.NotEqual("Hello Foo!", instance.SayHello()); // "Hello Foo!" is [Foo()] result.
        Assert.IsAssignableFrom<Foo2>(instance);
    }
}