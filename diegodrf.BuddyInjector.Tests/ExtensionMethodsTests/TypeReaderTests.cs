using diegodrf.BuddyInjector.ExtensionMethods;

namespace diegodrf.BuddyInjector.Tests.ExtensionMethodsTests;

public class TypeReaderTests
{
    [Fact]
    public void Given_A_Type_With_No_Generics_Should_Return_The_Name_Of_Type()
    {
        // Arrange
        var foo = new Foo();
        
        // Act
        var sut = foo.GetType().GetFormattedTypeName();
        
        // Assert
        Assert.Equal("Foo", sut);
    }
    
    [Fact]
    public void Given_A_Type_Generics_Should_Return_The_Name_Of_Type_And_Generics()
    {
        // Arrange
        var loggerService = new LoggerService<int, DateTime>();
        
        // Act
        var sut = loggerService.GetType().GetFormattedTypeName();
        
        // Assert
        Assert.Equal("LoggerService`2<Int32, DateTime>", sut);
    }

    [Fact] public void Given_A_Type_Null_Should_Return_Null()
    {
        // Arrange
        string? foo = null;
        
        // Act
        var sut = foo?.GetType().GetFormattedTypeName();
        
        // Assert
        Assert.Null(sut);
    }
}