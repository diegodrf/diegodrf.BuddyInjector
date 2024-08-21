# BuddyInjector
![CI](https://github.com/diegodrf/diegodrf.BuddyInjector/actions/workflows/dotnet.yml/badge.svg?branch=main)

Buddy Injector was designed to simplify unit testing processes that involve numerous injections.

Buddy Injector intends to be as simple as possible. It does not intend to be a robust dependency injector, to be used in
large scale, or as your main dependency container.

## Quick Start

### Installation 
```
dotnet add package diegodrf.BuddyInjector
```
### BuddyInjector class
The `BuddyInjector` class is the main point to interact with the dependencies. Here you will be able to register and get the dependencies.
#### Dispose resources
It's possible and recommended to call `Dispose` after using the container. Disposing `BuddyInjector` will call Dispose for all dependencies that implemented it.
```cs
BuddyInjector buddyInjector = new BuddyInjector();
// ...
buddyInjector.Dispose();
```
You can use the `using` statement.
```cs
using (BuddyInjector buddyInjector = new BuddyInjector())
{
    // ...
}
```
### Register
BuddyInjector allows you to register two life cycles, **Transient** and **Singleton**. Boot accepts two ways, implicit and explicit.
#### Register Singleton
- `RegisterSingleton<TType, TImplementation>()`
- `RegisterSingleton<T>(Func<T> instanceBuilder)`

```cs
using (BuddyInjector buddyInjector = new BuddyInjector())
{
    buddyInjector.RegisterSingleton<IFoo, Foo>();
    buddyInjector.RegisterSingleton<IFoo>(() => new Foo());
}

interface IFoo { }
class Foo : IFoo { }
```
#### Register Transient
- `RegisterTransient<TType, TImplementation>()`
- `RegisterTransient<T>(Func<T> instanceBuilder)`

```cs
using (BuddyInjector buddyInjector = new BuddyInjector())
{
    buddyInjector.RegisterTransient<IFoo, Foo>();
    buddyInjector.RegisterTransient<IFoo>(() => new Foo());
}

interface IFoo { }
class Foo : IFoo { }
```
#### Register All
When we create tests it's normal to instantiate the same class a lot of times to cover all the tests. So to help with this boring task BuddyInjector has the `RegisterAll`.

The `RegisterAll` method expects an `Action` that receives an instance of `BuddyInjector`. So you can create a base implementation to instantiate all your classes, like the real implementations for example, and substitute them per test with the specific mocks.
```cs
using diegodrf.BuddyInjector;

Action<BuddyInjector> instantiateAll = buddyInjector =>
{
    buddyInjector.RegisterSingleton<IFoo, Foo>();
    buddyInjector.RegisterSingleton<IBar, Bar>();
};

using (BuddyInjector buddyInjector = new BuddyInjector())
{
    buddyInjector.RegisterAll(instantiateAll);

    buddyInjector.GetInstance<IBar>();
}

interface IFoo { }
interface IBar { }
class Foo : IFoo { }
class Bar(IFoo foo) : IBar { }
```
#### Override instances
The behavior of **Registers** is to override registered instances. It allows you to register the real implementations and for each test that you need a mock you can replace it with it.
```cs
using (BuddyInjector buddyInjector = new BuddyInjector())
{
    // Register real implementation
    buddyInjector.RegisterSingleton<IFoo, RealImplementation>();

    // Register mocked implementation
    buddyInjector.RegisterSingleton<IFoo, MockImplementation>();

    // Get IFoo
    IFoo foo = buddyInjector.GetInstance<IFoo>();

    Console.WriteLine(foo is RealImplementation); // False
    Console.WriteLine(foo is MockImplementation); // True
}

interface IFoo { }
class RealImplementation : IFoo { }
class MockImplementation : IFoo { }
```
#### MultipleConstructorsException
To use the implicit registration BuddyInjector expects that the class has only one constructor. If there is more than one, it will throw `MultipleConstructorsException`.

When the class has more than one constructor, you should register using the explicit constructor.
```cs
using (BuddyInjector buddyInjector = new BuddyInjector())
{
    buddyInjector.RegisterSingleton<IFoo>(() => new Foo(true));
}

interface IFoo { }
class Foo : IFoo 
{
    public Foo()
    {
        
    }

    public Foo(bool someValue)
    {
        
    }
}
```

### Get instances
To retrieve the instances you use RegisterTransient `GetInstance<T>()`.
```cs
using (BuddyInjector buddyInjector = new BuddyInjector())
{
    buddyInjector.RegisterTransient<IFoo, Foo>();
    IFoo foo = buddyInjector.GetInstance<IFoo>();
}

interface IFoo { }
class Foo : IFoo { }
```
If you try to retrieve an object not registered, you will receive the exception `NotRegisteredException`.


