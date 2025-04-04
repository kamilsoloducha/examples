using System.Reflection;

namespace Testing;

public static class Utils
{
    private const string BackingFieldTemplate = "<{0}>k__BackingField";

    public static void SetField<TIn, TType>(this TIn obj, string filedName, TType value)
    {
        ArgumentNullException.ThrowIfNull(obj);
        
        var type = obj.GetType();
        var field = type.GetField(filedName, BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new InvalidOperationException($"There is no field {filedName}");
        field.SetValue(obj, value);
    }

    public static void SetProperty<TIn, TType>(this TIn obj, string propertyName, TType value)
    {
        ArgumentNullException.ThrowIfNull(obj);
        
        var backingFieldName = string.Format(BackingFieldTemplate, propertyName);
        obj.SetField(backingFieldName, value);
    }

    public static object? Execute<TIn>(this TIn obj, string functionName, object[] functionParams)
    {
        ArgumentNullException.ThrowIfNull(obj);
        
        var type = obj.GetType();
        var function = type.GetMethod(functionName, BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new InvalidOperationException($"There is no function {functionName}");
        return function.Invoke(obj, functionParams);
    }
}