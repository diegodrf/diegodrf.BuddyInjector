﻿namespace diegodrf.BuddyInjector.Tests;

public class BuddyInjectorTests
{
    [Fact]
    public void Should_Create_A_Singleton()
    {
        // Arrange
        var sut = new BuddyInjector();
        
        // Act
        sut.RegisterSingleton(() => new Foo());
        var a = sut.GetInstance<Foo>();
        var b = sut.GetInstance<Foo>();
        
        // Assert
        Assert.Equal(a, b);
    }
    
    [Fact]
    public void Should_Create_A_Transient()
    {
        // Arrange
        var sut = new BuddyInjector();
        
        // Act
        sut.RegisterTransient(() => new Foo());
        var a = sut.GetInstance<Foo>();
        var b = sut.GetInstance<Foo>();
        
        // Assert
        Assert.NotEqual(a, b);
    }

    [Fact]
    public void Should_Create_An_Object_Based_On_Interface()
    {
        // Arrange
        var sut = new BuddyInjector();
        
        // Act
        sut.RegisterSingleton<IFoo>(() => new Foo());
        var value = sut.GetInstance<IFoo>();
        
        // Assert
        Assert.IsAssignableFrom<IFoo>(value);
    }
    
    [Fact]
    public void Should_Return_NotRegisteredException_When_Get_An_Object_Not_Registered()
    {
        // Arrange
        var sut = new BuddyInjector();
        
        // Act
        
        // Assert
        Assert.Throws<NotRegisteredException>(() => sut.GetInstance<IBar>());
    }
    
    [Fact]
    public void Should_Register_All_Instances_From_Delegate_Function()
    {
        // Arrange
        
        // Act
        var sut = new BuddyInjector()
            .RegisterAll(x =>
        {
            x.RegisterSingleton<IFoo>(() => new Foo());
            x.RegisterSingleton<IBar>(() => new Bar(x.GetInstance<IFoo>()));
        });

        var instance = sut.GetInstance<IBar>();

        // Assert
        Assert.IsAssignableFrom<IBar>(instance);
    }
    
    [Fact]
    public void Should_Register_All_Instances_No_Matter_The_Order()
    {
        // Arrange
        var sut = new BuddyInjector();
        
        // Act
        sut.RegisterTransient<IBar>(() => new Bar(sut.GetInstance<IFoo>()));
        sut.RegisterTransient<IFoo>(() => new Foo());

        var message = sut.GetInstance<IBar>().Foo.SayHello();
        
        // Assert
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
    
    [Fact]
    public void Should_Register_All_Instances_Using_Auto_Map_Constructor_Parameters()
    {
        // Arrange
        var sut = new BuddyInjector();
        
        // Act
        sut.RegisterTransient<IFoo, Foo>();
        sut.RegisterTransient<IBar, Bar>();

        var message = sut.GetInstance<IBar>().Foo.SayHello();
        
        // Assert
        Assert.Equal("Hello Foo!", message);
    }
    
    [Fact]
    public void Should_Register_All_Instances_Using_Auto_Map_Constructor_Parameters_No_Matter_The_Order()
    {
        // Arrange
        var sut = new BuddyInjector();
        
        // Act
        sut.RegisterTransient<IBar, Bar>();
        sut.RegisterTransient<IFoo, Foo>();
        
        var message = sut.GetInstance<IBar>().Foo.SayHello();
        
        // Assert
        Assert.Equal("Hello Foo!", message);
    }
    
    [Fact]
    public void Should_Create_A_Singleton_Using_Auto_Map_Constructor_Parameters()
    {
        // Arrange
        var sut = new BuddyInjector();
        
        // Act
        sut.RegisterSingleton<IFoo, Foo>();
        var a = sut.GetInstance<IFoo>();
        var b = sut.GetInstance<IFoo>();
        
        // Assert
        Assert.Equal(a, b);
    }
    
    [Fact]
    public void Should_Create_A_Transient_Using_Auto_Map_Constructor_Parameters()
    {
        // Arrange
        var sut = new BuddyInjector();
        
        // Act
        sut.RegisterTransient<IFoo, Foo>();
        var a = sut.GetInstance<IFoo>();
        var b = sut.GetInstance<IFoo>();
        
        // Assert
        Assert.NotEqual(a, b);
    }

    [Fact]
    public void Given_A_Class_With_Multiple_Constructors_Should_Throw_An_Exception()
    {
        // Arrange
        var sut = new BuddyInjector();
        
        // Act
        
        // Assert
        Assert.Throws<MultipleConstructorsException>(() => sut.RegisterTransient<Car, Car>());
    }

    [Fact]
    public void Given_An_Instance_Should_Throw_An_Exception_When_A_Dependency_Is_Missing()
    {
        // Arrange
        var sut = new BuddyInjector();
        sut.RegisterTransient<IBar, Bar>();
        
        // Act
        
        // Assert
        Assert.Throws<NotRegisteredException>(() => sut.GetInstance<IBar>());
    }
    
    [Fact]
    public void Given_A_Disposable_Object_To_Container_As_Singleton_Should_Disposed_Objects_When_Scope_Ends()
    {
        // Arrange
        var sut = new BuddyInjector();
        sut.RegisterSingleton<DisposableClass, DisposableClass>();
        sut.GetInstance<DisposableClass>();
        
        // Act
        sut.Dispose();
        
        // Assert
        Assert.Throws<NotRegisteredException>(sut.GetInstance<DisposableClass>);
    }
    
    [Fact]
    public void Should_Dispose_Objects_When_Run_In_Using_Scope()
    {
        // Arrange
        
        // Act
        using (var sut = new BuddyInjector()) 
        {
            sut.RegisterSingleton<DisposableClass, DisposableClass>();
            sut.GetInstance<DisposableClass>();
        }

        // Assert
        Assert.True(DisposableClass.Disposed);
    }

    [Fact]
    public void Should_Throw_An_Exception_When_Register_A_Singleton_Using_An_Interface_Instead_A_Concrete_Class()
    {
        // Arrange
        var sut = new BuddyInjector();

        // Act
        void register() => sut.RegisterSingleton<IBar, IBar>();

        // Assert
        Assert.Throws<ArgumentException>(register);
    }

    [Fact]
    public void Should_Throw_An_Exception_When_Register_A_Transient_Using_An_Interface_Instead_A_Concrete_Class()
    {
        // Arrange
        var sut = new BuddyInjector();

        // Act
        void register() => sut.RegisterTransient<IBar, IBar>();

        // Assert
        Assert.Throws<ArgumentException>(register);
    }

    [Fact]
    public void Should_Throw_An_Exception_With_Clear_Message_When_Register_An_Interface_Instead_A_Concrete_Class()
    {
        // Arrange
        var sut = new BuddyInjector();

        // Act
        void register() => sut.RegisterTransient<IBar, IBar>();

        // Assert
        Assert.Equal("IBar is not a concrete class.", Assert.ThrowsAny<Exception>(register).Message);
    }
}