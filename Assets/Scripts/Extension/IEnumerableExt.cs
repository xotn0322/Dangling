using System.Collections;
using System.Collections.Generic;

public static class IEnumerableExt
{
    public static string ConcatStringExt<T>(this IEnumerable<T> e, string seperator)
    {
        if (e == null)
        {
            return null;
        }

        var builder = new System.Text.StringBuilder();
        foreach (var s in e)
        {
            if (builder.Length > 0)
            {
                builder.Append(seperator);
            }

            builder.Append(s);
        }

        return builder.ToString();
    }

    public static string ConcatStringExt<T>(this IEnumerable<T> e, char seperator)
    {
        if (e == null)
        {
            return null;
        }

        var builder = new System.Text.StringBuilder();
        foreach (var s in e)
        {
            if (builder.Length > 0)
            {
                builder.Append(seperator);
            }

            builder.Append(s);
        }

        return builder.ToString();
    }

    public static string ConcatStringExt<T>(this IEnumerable<T> e)
    {
        return ConcatStringExt(e, ',');
    }

    public static string ConcatStringExt(this IEnumerable e, string seperator)
    {
        if (e == null)
        {
            return null;
        }

        var builder = new System.Text.StringBuilder();
        foreach (var s in e)
        {
            if (builder.Length > 0)
            {
                builder.Append(seperator);
            }

            builder.Append(s);
        }

        return builder.ToString();
    }

    public static string ConcatStringExt(this IEnumerable e)
    {
        return ConcatStringExt(e, ',');
    }

    public static string ConcatStringExt(this IEnumerable e, char seperator)
    {
        if (e == null)
        {
            return null;
        }

        var builder = new System.Text.StringBuilder();
        foreach (var s in e)
        {
            if (builder.Length > 0)
            {
                builder.Append(seperator);
            }

            builder.Append(s);
        }

        return builder.ToString();
    }

    public static string ConcatStringAsLocalKeyExt<T>(this IEnumerable<T> e, char seperator)
    {
        if (e == null)
        {
            return null;
        }

        var builder = new System.Text.StringBuilder();
        int index = 0;

        foreach (var _ in e)
        {
            if (builder.Length > 0)
            {
                builder.Append(seperator);
            }

            builder.Append($"{{{index}}}");
            index++;
        }

        return builder.ToString();
    }

    public static string ConcatStringAsLocalKeyExt<T>(this IEnumerable<T> e, string seperator)
    {
        if (e == null)
        {
            return null;
        }

        var builder = new System.Text.StringBuilder();
        int index = 0;

        foreach (var _ in e)
        {
            if (builder.Length > 0)
            {
                builder.Append(seperator);
            }

            builder.Append($"{{{index}}}");
            index++;
        }

        return builder.ToString();
    }

    public static string ConcatStringAsLocalKeyExt<T>(this IEnumerable<T> e)
    {
        return ConcatStringAsLocalKeyExt(e, ',');
    }

    public static string ConcatStringAsLocalKeyExt(this IEnumerable e, char seperator)
    {
        if (e == null)
        {
            return null;
        }

        var builder = new System.Text.StringBuilder();
        int index = 0;

        foreach (var s in e)
        {
            if (builder.Length > 0)
            {
                builder.Append(seperator);
            }

            builder.Append($"{{{index}}}");
            index++;
        }

        return builder.ToString();
    }

    public static string ConcatStringAsLocalKeyExt(this IEnumerable e, string seperator)
    {
        if (e == null)
        {
            return null;
        }

        var builder = new System.Text.StringBuilder();
        int index = 0;

        foreach (var s in e)
        {
            if (builder.Length > 0)
            {
                builder.Append(seperator);
            }

            builder.Append($"{{{index}}}");
            index++;
        }

        return builder.ToString();
    }

    public static string ConcatStringAsLocalKeyExt(this IEnumerable e)
    {
        return ConcatStringAsLocalKeyExt(e, ',');
    }
}