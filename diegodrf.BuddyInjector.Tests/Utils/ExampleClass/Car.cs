namespace diegodrf.BuddyInjector.Tests.Utils.ExampleClass;

public class Car
{
    public string Name { get; set; }

    public Car()
    {
        Name = string.Empty;
    }

    public Car(string name)
    {
        Name = name;
    }
}