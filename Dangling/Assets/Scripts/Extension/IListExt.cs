using System;
using System.Collections.Generic;

public static class IListExt
{
    public static void RemoveFisrtExt<T>(this IList<T> list, Predicate<T> match)
    {
        if (list == null)
        {
            return;
        }

        for (int i = 0; i <list.Count; ++i)
        {
            if (match(list[i]))
            {
                list.RemoveAt(i);
                return;
            }
        }
    }

    public static void RemoveAllExt<T>(this IList<T> list, Predicate<T> match)
    {
        if (list == null)
        {
            return;
        }

        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (match(list[i]))
            {
                list.RemoveAt(i);
            }
        }
    }

    public static bool TryGetValueExt<T>(this IList<T> list, Predicate<T> predicate, out T data)
    {
        if (list == null)
        {
            data = default;
            return false;
        }

        foreach (T item in list)
        {
            if (predicate(item))
            {
                data = item;
                return true;
            }
        }

        data = default;
        return false;
    }
}