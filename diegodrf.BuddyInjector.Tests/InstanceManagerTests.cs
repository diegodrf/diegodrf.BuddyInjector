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
}