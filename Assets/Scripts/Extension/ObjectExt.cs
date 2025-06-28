using System.Collections.Generic;
using System.Reflection;

public static class ObjectExt
{
    public static T CloneProperties<T>(this T source, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        where T : class, new()
    {
        if (source == null)
        {
            return null;
        }

        var cloned = new T();
        var type = typeof(T);
        foreach (var property in type.GetProperties(bindingFlags))
        {
            var sourceValue = property.GetValue(source);
            if (sourceValue != null)
            {
                property.SetValue(cloned, sourceValue);
            }
        }

        return cloned;
    }

    public static bool EqualsExt<T>(this T e, T other)
    {
        return EqualityComparer<T>.Default.Equals(e, other);
    }
}