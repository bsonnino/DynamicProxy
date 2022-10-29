using System.Reflection;

class RepositoryLogger<T> : DispatchProxy where T : class
{
    T? _decorated;

    public T? Create(T decorated)
    {
        var proxy = Create<T, RepositoryLogger<T>>() as RepositoryLogger<T>;
        if (proxy != null)
        {
            proxy._decorated = decorated;
        }
        return proxy as T;
    }


    protected override object? Invoke(MethodInfo? methodInfo, object?[]? args)
    {
        if (methodInfo == null)
        {
            return null;
        }

        Log($"Entering {methodInfo.Name}");
        try
        {
            var result = methodInfo.Invoke(_decorated, args);
            Log($"Exiting {methodInfo.Name}");
            return result;
        }
        catch
        {
            Log($"Error {methodInfo.Name}");
            throw;
        }
    }

    private static void Log(string msg)
    {
        Console.ForegroundColor = msg.StartsWith("Entering") ? ConsoleColor.Blue :
            msg.StartsWith("Exiting") ? ConsoleColor.Green : ConsoleColor.Red;
        Console.WriteLine(msg);
        Console.ResetColor();
    }
}