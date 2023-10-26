namespace diegodrf.BuddyInjector;

public class InstanceManager<T>
{
    private T? _instance;
    private readonly Func<T> _instanceBuilder;
    private readonly bool _isSingleton;
    
    public InstanceManager(Func<T> instanceBuilder, bool isSingleton = false)
    {
        _isSingleton = isSingleton;
        _instanceBuilder = instanceBuilder;
    }
    
    public T GetInstance()
    {
        if (!_isSingleton) return _instanceBuilder.Invoke();
        _instance ??= _instanceBuilder.Invoke();
        return _instance;
    }
}