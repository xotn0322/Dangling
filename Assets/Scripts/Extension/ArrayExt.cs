
using System.Collections.Generic;
using System;

public static class ArrayExt
{
    public static int GetCountExt<T>(this T[] collection)
    {
        return collection != null ? collection.Length : 0;
    }

    public static bool IsEmptyExt<T>(this T[] collection)
    {
        return collection == null || collection.Length == 0;
    }

    public static int FindIndexExt<T>(this T[] collection, T item, IEqualityComparer<T> comparer = null)
    {
        if (collection == null)
        {
            return -1;
        }

        for (var i = 0; i < collection.Length; i++)
        {
            var v = collection[i];
            if (comparer != null)
            {
                if (comparer.Equals(v, item))
                {
                    return i;
                }
            }
            else
            {
                if (v.EqualsExt(item))
                {
                    return i;
                }
            }
        }

        return -1;
    }

    public static int FindIndexExt<T>(this T[] collection, Predicate<T> predicate)
    {
        if (collection == null)
        {
            return -1;
        }

        for (var i = 0; i < collection.Length; i++)
        {
            var v = collection[i];
            if (predicate(v))
            {
                return i;
            }
        }

        return -1;
    }

    public static T FindExt<T>(this T[] collection, Predicate<T> predicate)
    {
        if (collection == null)
        {
            return default;
        }

        for (var i = 0; i < collection.Length; i++)
        {
            var v = collection[i];
            if (predicate(v))
            {
                return v;
            }
        }

        return default;
    }

    public static bool ContainsExt<T>(this T[] collection, T item)
    {
        return FindIndexExt(collection, item) >= 0;
    }

    public static bool ContainsExt<T>(this T[] collection, Predicate<T> predicate)
    {
        return FindIndexExt(collection, predicate) >= 0;
    }

    public static bool ContainsAnyExt<T>(this T[] collection, IEnumerable<T> items, IEqualityComparer<T> comparer = null)
    {
        if (collection == null)
        {
            return false;
        }

        if (items == null)
        {
            return false;
        }

        foreach (var e in collection)
        {
            foreach (var item in items)
            {
                if (comparer != null)
                {
                    if (comparer.Equals(e, item))
                    {
                        return true;
                    }
                }
                else
                {
                    if (item.EqualsExt(e))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public static T[] SpliceExt<T>(this T[] collection, int offset, int length = -1)
    {
        if (collection == null)
        {
            return null;
        }

        var remainLength = collection.Length - offset;
        if (length != -1)
        {
            remainLength = Math.Min(remainLength, length);
        }

        if (remainLength <= 0)
        {
            return null;
        }

        var result = new T[remainLength];
        for (var i = 0; i < remainLength; i++)
        {
            result[i] = collection[offset + i];
        }

        return result;
    }

    public static T[] ExtendToArrayExt<T>(this T[] collection, T item)
    {
        if (collection == null)
        {
            return new T[] { item };
        }

        var length = collection.Length;
        var array = new T[length + 1];
        var index = 0;
        for (; index < length; index++)
        {
            var e = collection[index];
            array[index] = e;
        }

        array[index] = item;
        return array;
    }

    public static T[] ExcludeFromArrayExt<T>(this T[] collection, T item)
    {
        if (collection == null || collection.Length == 0)
        {
            return collection;
        }

        var foundIndex = collection.IndexOfExt(item);
        if (foundIndex == -1)
        {
            return collection;
        }

        var length = collection.Length;
        var array = new T[length - 1];
        for (var i = 0; i < length; i++)
        {
            if (i < foundIndex)
            {
                array[i] = collection[i];
            }
            else if (i > foundIndex)
            {
                array[i - 1] = collection[i];
            }
        }

        return array;
    }

    public static void ForEachExt<T>(this T[] collection, Action<T> action)
    {
        if (collection == null)
        {
            return;
        }

        for (var i = 0; i < collection.Length; i++)
        {
            action.RunExt(collection[i]);
        }
    }
}