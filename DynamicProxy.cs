using System.Reflection;

class DynamicProxy<T> : DispatchProxy where T : class
{
    T? _decorated;
    private Action<MethodInfo>? _beforeExecute;
    private Action<MethodInfo>? _afterExecute;
    private Action<MethodInfo>? _onError;
    private Predicate<MethodInfo> _shouldExecute;

    public T? Create(T decorated, Action<MethodInfo>? beforeExecute, 
        Action<MethodInfo>? afterExecute, Action<MethodInfo>? onError, 
        Predicate<MethodInfo>? shouldExecute)
    {
        var proxy = Create<T, DynamicProxy<T>>() as DynamicProxy<T>;
        if (proxy == null)
        {
            return null;
        }
        proxy._decorated = decorated;
        proxy._beforeExecute = beforeExecute;
        proxy._afterExecute = afterExecute;
        proxy._onError = onError;
        proxy._shouldExecute = shouldExecute ?? (s => true);
        return proxy as T;
    }

    protected override object? Invoke(MethodInfo? methodInfo, object?[]? args)
    {
        if (methodInfo == null)
        {
            return null;
        }
        if (!_shouldExecute(methodInfo))
        {
            return null;
        }
        _beforeExecute?.Invoke(methodInfo);
        try
        {
            var result = methodInfo.Invoke(_decorated, args);
            _afterExecute?.Invoke(methodInfo);
            return result;
        }
        catch
        {
            _onError?.Invoke(methodInfo);
            throw;
        }
    }
}