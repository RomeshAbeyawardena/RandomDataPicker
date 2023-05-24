using System.Reflection;

namespace RandomDataPicker.Extensions;

public static class ObjectExtensions
{
    private static readonly IDictionary<Type, IEnumerable<PropertyInfo>> TypeProperties = new Dictionary<Type, IEnumerable<PropertyInfo>>();
    
    public static T? ShallowCopy<T>(this T value)
        where T : class
    {
        var type = typeof(T);
        
        if (!TypeProperties.TryGetValue(type, out var props))
        {
            props = type.GetProperties();
            TypeProperties.Add(type, props);
        }

        var newValue = Activator.CreateInstance(typeof(T));

        foreach(var property in props)
        {
            if(property.CanRead && property.CanWrite)
            {
                var val = property.GetValue(value);
                if (val != null)
                {
                    property.SetValue(newValue, val);
                }
            }
        }

        return newValue as T;
    }
}
