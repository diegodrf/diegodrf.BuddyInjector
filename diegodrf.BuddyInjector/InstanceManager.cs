namespace diegodrf.BuddyInjector;

public class InstanceManager
{
    private object? _instance;
    private readonly Func<object> _instanceBuilder;
    private readonly bool _isSingleton;
    
    public InstanceManager(Func<object> instanceBuilder, bool isSingleton = false)
    {
        _isSingleton = isSingleton;
        _instanceBuilder = instanceBuilder;
    }
    
    public object GetInstance()
    {
        if (!_isSingleton) return _instanceBuilder.Invoke();
        _instance ??= _instanceBuilder.Invoke();
        return _instance;
    }
}