﻿using System.Text;
using diegodrf.BuddyInjector.Exceptions;
using diegodrf.BuddyInjector.ExtensionMethods;

namespace diegodrf.BuddyInjector;

public class BuddyInjector : IDisposable
{
    private bool _disposed;
    private readonly Dictionary<Type, InstanceManager> _instanceMap = new();

    private void Register<T>(Func<T> instanceBuilder, bool isSingleton)
    {
        var func = instanceBuilder as Func<object>;
        _instanceMap[typeof(T)] = new InstanceManager(func!, isSingleton);
    }

    private void Register<TType, TImp>(bool isSingleton)
    {
        if(typeof(TImp).IsInterface || typeof(TImp).IsAbstract)
        {
            throw new ArgumentException($"{typeof(TImp).Name} is not a concrete class.");
        }

        var constructors = typeof(TImp).GetConstructors();

        if (constructors.Length > 1)
        {
            var message = new StringBuilder()
                .Append($"It's not possible to determine the correct constructor for {typeof(TImp).Name}. ")
                .Append("You should explicitly instantiate it manually.")
                .ToString();
            throw new MultipleConstructorsException(message);
        }
        
        Register<TType>(() =>
        {
            var constructor = constructors[0];

            var parameters = constructor
                .GetParameters()
                .Select(x =>
                {
                    if (_instanceMap.TryGetValue(x.ParameterType, out var value))
                    {
                        return value.GetInstance();
                    }

                    var message =
                        $"It's not possible to initiate {typeof(TImp).Name} "
                        + $"because the dependency {x.ParameterType.GetFormattedTypeName()} is not registered.";
                    throw new NotRegisteredException(message);
                })
                .ToArray();
            
            return (TType)constructor.Invoke(parameters.ToArray());
        }, isSingleton);
    }

    /// <summary>
    /// Register an object as Singleton life scope.
    /// </summary>
    /// <param name="instanceBuilder">Anonymous function to create the instance.</param>
    /// <typeparam name="T">The type that will be injected.</typeparam>
    public void RegisterSingleton<T>(Func<T> instanceBuilder)
    {
        Register(instanceBuilder, true);
    }
    
    /// <summary>
    /// Register an object as Transient life scope.
    /// </summary>
    /// <param name="instanceBuilder">Anonymous function to create the instance.</param>
    /// <typeparam name="T">The type that will be injected.</typeparam>
    public void RegisterTransient<T>(Func<T> instanceBuilder)
    {
        Register(instanceBuilder, false);
    }

    /// <summary>
    /// Register an object as Singleton life scope.
    /// </summary>
    /// <typeparam name="TType">The type that will be injected.</typeparam>
    /// <typeparam name="TImplementation">The type of real implementation.</typeparam>
    public void RegisterSingleton<TType, TImplementation>()
    {
        Register<TType, TImplementation>(true);
    }
    
    /// <summary>
    /// Register an object as Transient life scope.
    /// </summary>
    /// <typeparam name="TType">The type that will be injected.</typeparam>
    /// <typeparam name="TImplementation">The type of real implementation.</typeparam>
    public void RegisterTransient<TType, TImplementation>()
    {
        Register<TType, TImplementation>(false);
    }

    /// <summary>
    /// Get a instance of [T] or return an exception.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="NotRegisteredException"></exception>
    public T GetInstance<T>()
    {
        if (_instanceMap.TryGetValue(typeof(T), out var value))
        {
            return (T)value.GetInstance();
        }

        throw new NotRegisteredException($"[{typeof(T).Name}] is not registered.");
    }

    /// <summary>
    /// Register all instances using an [Action].
    /// Use it when you need to repeat the same registration on different calls.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public BuddyInjector RegisterAll(Action<BuddyInjector> action)
    {
        action.Invoke(this);
        return this;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }
        
        if (disposing)
        {
            foreach (var instance in _instanceMap)
            {
                if (instance.Value.IsDisposable)
                {
                    ((IDisposable)instance.Value.GetInstance()).Dispose();
                }
            }
        }
        
        _instanceMap.Clear();
        
        _disposed = true;
    }
}