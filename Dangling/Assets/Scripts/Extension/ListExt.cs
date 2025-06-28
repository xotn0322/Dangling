using System;
using System.Collections.Generic;


public static class ListExtension
{
    public static int GetCountExt<T>(this IReadOnlyList<T> collection)
    {
        return collection != null ? collection.Count : 0;
    }

    public static bool IsEmptyExt<T>(this IReadOnlyList<T> collection)
    {
        return collection == null || collection.Count == 0;
    }

    public static bool IsEmptyExt<T>(this List<T>.Enumerator e)
    {
        while (e.MoveNext())
        {
            return false;
        }

        return true;
    }

    public static int IndexOfExt<T>(this IReadOnlyList<T> collection, T item, IEqualityComparer<T> comparer = null)
    {
        if (collection == null)
        {
            return -1;
        }

        for (var i = 0; i < collection.Count; i++)
        {
            var e = collection[i];
            if (comparer != null)
            {
                if (comparer.Equals(e, item))
                {
                    return i;
                }
            }
            else
            {
                if (e.EqualsExt(item))
                {
                    return i;
                }
            }
        }

        return -1;
    }

    public static int FindIndexExt<T>(this IReadOnlyList<T> collection, Predicate<T> predicate)
    {
        if (collection == null)
        {
            return -1;
        }

        for (var i = 0; i < collection.Count; i++)
        {
            var e = collection[i];
            if (predicate(e))
            {
                return i;
            }
        }

        return -1;
    }

    public static T FindExt<T>(this IReadOnlyList<T> collection, Predicate<T> predicate)
    {
        if (collection == null)
        {
            return default;
        }

        for (var i = 0; i < collection.Count; i++)
        {
            var e = collection[i];
            if (predicate(e))
            {
                return e;
            }
        }

        return default;
    }

    public static bool ContainsExt<T>(this IReadOnlyList<T> collection, T item, IEqualityComparer<T> comparer = null)
    {
        if (collection == null)
        {
            return false;
        }

        for (var i = 0; i < collection.Count; i++)
        {
            var e = collection[i];
            if (comparer != null)
            {
                if (comparer.Equals(e, item))
                {
                    return true;
                }
            }
            else
            {
                if (e.EqualsExt(item))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public static bool ContainsExt<T>(this IReadOnlyList<T> collection, Predicate<T> predicate)
    {
        if (collection == null)
        {
            return false;
        }

        for (var i = 0; i < collection.Count; i++)
        {
            var e = collection[i];
            if (predicate(e))
            {
                return true;
            }
        }

        return false;
    }

    public static T GetElementExt<T>(this IReadOnlyList<T> e, int index)
    {
        return (e != null && index >= 0 && index < e.Count) ? e[index] : default;
    }

    public static bool SetElementExt<T>(this IList<T> e, int index, T value)
    {
        if (e != null && index >= 0 && index < e.Count)
        {
            e[index] = value;
            return true;
        }

        return false;
    }

    public static T GetFirstExt<T>(this IReadOnlyList<T> e)
    {
        return GetElementExt(e, 0);
    }

    public static T GetFirstExt<T>(this List<T>.Enumerator e)
    {
        while (e.MoveNext())
        {
            return e.Current;
        }

        return default;
    }

    public static T GetLastExt<T>(this IReadOnlyList<T> e)
    {
        if (e == null)
        {
            return default;
        }

        return GetElementExt(e, e.Count - 1);
    }

    public static T[] SpliceExt<T>(this IReadOnlyList<T> collection, int offset, int length = -1)
    {
        if (collection == null)
        {
            return null;
        }

        var remainLength = collection.Count - offset;
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

    public static void RemoveExt<T>(this IList<T> collection, T item)
    {
        if (collection == null)
        {
            return;
        }

        collection.Remove(item);
    }

    public static void RemoveBackwardExt<T>(this IList<T> e, IReadOnlyList<int> ascendingIndices)
    {
        var prevIndex = int.MaxValue;
        for (var i = ascendingIndices.GetCountExt() - 1; i >= 0; i--)
        {
            var index = ascendingIndices[i];
            if (index > prevIndex)
            {
                Logger.Error($"ListExt : RemoveBackward skipped. list is not ascending. T={typeof(T)}. index={index}. prevIndex={prevIndex}", typeof(ListExtension));
                continue;
            }

            e.RemoveAt(index);
            prevIndex = index;
        }
    }

    public static void RemoveAllExt<T>(this List<T> collection, Predicate<T> predicate)
    {
        if (collection == null)
        {
            return;
        }

        collection.RemoveAll(predicate);
    }
}