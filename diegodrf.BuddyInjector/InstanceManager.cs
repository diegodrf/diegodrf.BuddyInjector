namespace diegodrf.BuddyInjector;

public class InstanceManager
{
    private object? _instance;
    private readonly Func<object> _instanceBuilder;
    private readonly bool _isSingleton;
    private bool _isDisposable;
    public bool IsDisposable => _isDisposable;
    
    
    public InstanceManager(Func<object> instanceBuilder, bool isSingleton = false)
    {
        _isSingleton = isSingleton;
        _instanceBuilder = instanceBuilder;
    }
    
    public object GetInstance()
    {
        if (!_isSingleton) return _instanceBuilder.Invoke();
        
        if (_instance is null)
        {
            _instance = _instanceBuilder.Invoke();
            _isDisposable = _instance is IDisposable;
        }
        
        return _instance;
    }
}