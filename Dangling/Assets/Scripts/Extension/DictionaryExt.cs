using System.Collections.Generic;

public static class DictionaryExt
{
    public static bool IsEmptyExt<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary)
    {
        return dictionary == null || dictionary.Count == 0;
    }

    public static int GetCountExt<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary)
    {
        return dictionary != null ? dictionary.Count : 0;
    }

    public static bool ContainsKeyExt<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key)
    {
        return dictionary != null ? dictionary.ContainsKey(key) : false;
    }

    public static TValue GetValueExt<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default(TValue))
    {
        if (dictionary == null)
        {
            return defaultValue;
        }

        if (dictionary.TryGetValue(key, out var value))
        {
            return value;
        }

        return defaultValue;
    }

    public static bool TryGetValueExt<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key, out TValue value)
    {
        if (dictionary == null)
        {
            value = default(TValue);
            return false;
        }

        return dictionary.TryGetValue(key, out value);
    }

    public static bool RemoveExt<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
    {
        return TryRemoveExt(dictionary, key, out _);
    }

    public static bool TryRemoveExt<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, out TValue value)
    {
        if (dictionary == null)
        {
            value = default;
            return false;
        }

        if (dictionary.TryGetValue(key, out value))
        {
            dictionary.Remove(key);
            return true;
        }

        value = default;
        return false;
    }

    public static bool TryAddExt<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        if (dictionary == null)
        {
            return false;
        }

        if (dictionary.ContainsKey(key))
        {
            return false;
        }

        dictionary.Add(key, value);
        return true;
    }
}