using diegodrf.BuddyInjector.Exceptions;

namespace diegodrf.BuddyInjector;

public class BuddyInjector
{
    private readonly Dictionary<Type, object> _instanceMap = new();

    private void Register<T>(Func<T> instanceBuilder, bool isSingleton)
    {
        _instanceMap[typeof(T)] = new InstanceManager<T>(instanceBuilder, isSingleton);
    }

    /// <summary>
    /// Register an object as Singleton life scope.
    /// </summary>
    /// <param name="instanceBuilder"></param>
    /// <typeparam name="T"></typeparam>
    public void RegisterSingleton<T>(Func<T> instanceBuilder)
    {
        Register(instanceBuilder, true);
    }
    
    /// <summary>
    /// Register an object as Transient life scope.
    /// </summary>
    /// <param name="instanceBuilder"></param>
    /// <typeparam name="T"></typeparam>
    public void RegisterTransient<T>(Func<T> instanceBuilder)
    {
        Register(instanceBuilder, false);
    }

    /// <summary>
    /// Get a instance of [T] or return an exception.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="NotRegisteredException"></exception>
    public T GetInstance<T>()
    {
        if (
            _instanceMap.TryGetValue(typeof(T), out var value)
            && value is InstanceManager<T> instanceManager)
        {
            return instanceManager.GetInstance();
        };
        
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
}