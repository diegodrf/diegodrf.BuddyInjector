namespace diegodrf.BuddyInjector.Tests;

public class InstanceManagerTests
{
    [Fact]
    public void Should_Create_A_Singleton()
    {
        // Arrange
        
        // Act
        var sut = new InstanceManager(() => new Foo(), true);
        var a = sut.GetInstance();
        var b = sut.GetInstance();
        
        // Assert
        Assert.Equal(a, b);
    }
    
    [Fact]
    public void Should_Create_A_Transient()
    {
        // Arrange
        
        // Act
        var sut = new InstanceManager(() => new Foo(), false);
        var a = sut.GetInstance();
        var b = sut.GetInstance();
        
        // Assert
        Assert.NotEqual(a, b);
    }

    [Fact]
    public void Given_A_Disposable_Class_Should_Set_IsDisposable_When_The_Object_Is_Singleton()
    {
        // Arrange
        var sut = new InstanceManager(() => new DisposableClass(), true);
        
        // Act
        sut.GetInstance();
        
        // Assert
        Assert.True(sut.IsDisposable);
    }
    
    [Fact]
    public void Given_A_Disposable_Class_Should_Not_Set_IsDisposable_When_The_Object_Is_Transient()
    {
        // Arrange
        var sut = new InstanceManager(() => new DisposableClass(), false);
        
        // Act
        sut.GetInstance();
        
        // Assert
        Assert.False(sut.IsDisposable);
    }
    
    [Fact]
    public void Given_A_Non_Disposable_Class_Should_Not_Set_IsDisposable_When_The_Object_Is_Singleton()
    {
        // Arrange
        var sut = new InstanceManager(() => new Foo(), true);
        
        // Act
        sut.GetInstance();
        
        // Assert
        Assert.False(sut.IsDisposable);
    }
    
    [Fact]
    public void Given_A_Non_Disposable_Class_Should_Not_Set_IsDisposable_When_The_Object_Is_Transient()
    {
        // Arrange
        var sut = new InstanceManager(() => new Foo(), false);
        
        // Act
        sut.GetInstance();
        
        // Assert
        Assert.False(sut.IsDisposable);
    }
}