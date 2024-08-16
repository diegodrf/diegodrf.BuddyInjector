# diegodrf.BuddyInjector
![CI](https://github.com/diegodrf/diegodrf.BuddyInjector/actions/workflows/dotnet.yml/badge.svg?branch=main)

Buddy Injector intends to be as simple as possible. It does not intend to be a robust dependency injector, to be used in
large scale, or as your main dependency container.

Buddy Injector was created to help your manual job when unit testing your class that depends on many injections.

[Download](https://www.nuget.org/packages/diegodrf.BuddyInjector/)

### Examples
See [a simple project example](https://github.com/diegodrf/diegodrf.BuddyInjector/tree/main/diegodrf.BuddyInjector.ExampleProject).

## Quick Start
### BuddyInjector class
The `BuddyInjector` class is the main point to interact with the dependencies. Here you will be able to register and get the dependencies.
#### Dispose resources
It's possible and recommended to call `Dispose` after using the container. Disposing `BuddyInjector` will call Dispose for all dependencies that implementing it.
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
TODO
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


