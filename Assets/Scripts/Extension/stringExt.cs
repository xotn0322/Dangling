using System;
using System.Collections.Generic;
using System.Text;

public static class StringExt
{
    public const char SlashChar = '/';
    public static string Slash { get; } = $"{SlashChar}";

    #region check, count, find
    public static bool IsEmptyExt(this string s)
    {
        return s == null || s.Length == 0;
    }

    public static int GetCountExt(this string s)
    {
        return s != null ? s.Length : 0;
    }

    public static bool HasValueExt(this string s)
    {
        return s != null && s.Length != 0;
    }

    public static bool ContainsExt(this string s, string other)
    {
        if (s.IsEmptyExt())
        {
            return false;
        }

        var index = s.IndexOf(other);
        return index >= 0;
    }

    public static bool ContainsExt(this string s, char other)
    {
        if (s.IsEmptyExt())
        {
            return false;
        }

        var index = s.IndexOf(other);
        return index >= 0;
    }

    public static int IndexOfExt(this string s, string other)
    {
        if (s.IsEmptyExt())
        {
            return -1;
        }

        return s.IndexOf(other);
    }

    public static int IndexOfExt(this string s, char other)
    {
        if (s.IsEmptyExt())
        {
            return -1;
        }

        return s.IndexOf(other);
    }

    public static int IndexOfExt(this string s, char other, int start)
    {
        if (s.IsEmptyExt())
        {
            return -1;
        }

        return s.IndexOf(other, start);
    }

    public static bool StartsWithExt(this string s, string other)
    {
        if (s.IsEmptyExt())
        {
            return false;
        }

        return s.StartsWith(other);
    }

    public static bool StartsWithExt(this string s, char other)
    {
        if (s.IsEmptyExt())
        {
            return false;
        }

        return s.StartsWith(other);
    }

    public static bool EndsWithExt(this string s, string other)
    {
        if (s.IsEmptyExt())
        {
            return false;
        }

        return s.EndsWith(other);
    }

    public static bool EndsWithExt(this string s, char other)
    {
        if (s.IsEmptyExt())
        {
            return false;
        }

        return s.EndsWith(other);
    }
    #endregion

    #region type Convert
    public static bool TryToIntExt(this string s, out int value)
    {
        return int.TryParse(s, out value);
    }

    public static bool TryToIntExt(this string s, int start, out int value)
    {
        if (s.GetCountExt() <= start)
        {
            value = default;
            return false;
        }

        return int.TryParse(s.AsSpan(start), out value);
    }

    public static int ToIntExt(this string s, int defaultValue = 0)
    {
        if (TryToIntExt(s, out var value) == false)
        {
            return defaultValue;
        }

        return value;
    }

    public static bool TryToUintExt(this string s, out uint value)
    {
        return uint.TryParse(s, out value);
    }

    public static bool TryToUintExt(this string s, int start, out uint value)
    {
        if (s.GetCountExt() <= start)
        {
            value = default;
            return false;
        }

        return uint.TryParse(s.AsSpan(start), out value);
    }

    public static uint ToUintExt(this string s, uint defaultValue = 0)
    {
        if (TryToUintExt(s, out var value) == false)
        {
            return defaultValue;
        }

        return value;
    }

    public static bool TryToShortExt(this string s, out short value)
    {
        return short.TryParse(s, out value);
    }

    public static bool TryToShortExt(this string s, int start, out short value)
    {
        if (s.GetCountExt() <= start)
        {
            value = default;
            return false;
        }

        return short.TryParse(s.AsSpan(start), out value);
    }

    public static short ToShortExt(this string s, short defaultValue = 0)
    {
        if (TryToShortExt(s, out var value) == false)
        {
            return defaultValue;
        }

        return value;
    }

    public static bool TryToUshortExt(this string s, out ushort value)
    {
        return ushort.TryParse(s, out value);
    }

    public static bool TryToUshortExt(this string s, int start, out ushort value)
    {
        if (s.GetCountExt() <= start)
        {
            value = default;
            return false;
        }

        return ushort.TryParse(s.AsSpan(start), out value);
    }

    public static ushort ToUshortExt(this string s, ushort defaultValue = 0)
    {
        if (TryToUshortExt(s, out var value) == false)
        {
            return defaultValue;
        }

        return value;
    }

    public static bool TryToSbyteExt(this string s, out sbyte value)
    {
        return sbyte.TryParse(s, out value);
    }

    public static bool TryToSbyteExt(this string s, int start, out sbyte value)
    {
        if (s.GetCountExt() <= start)
        {
            value = default;
            return false;
        }

        return sbyte.TryParse(s.AsSpan(start), out value);
    }

    public static sbyte ToSbyteExt(this string s, sbyte defaultValue = 0)
    {
        if (TryToSbyteExt(s, out var value) == false)
        {
            return defaultValue;
        }

        return value;
    }

    public static bool TryToByteExt(this string s, out byte value)
    {
        return byte.TryParse(s, out value);
    }

    public static byte ToByteExt(this string s, byte defaultValue = 0)
    {
        if (TryToByteExt(s, out var value) == false)
        {
            return defaultValue;
        }

        return value;
    }

    public static bool ToByteExt(this string s, int start, out byte value)
    {
        if (s.GetCountExt() <= start)
        {
            value = default;
            return false;
        }

        return byte.TryParse(s.AsSpan(start), out value);
    }

    public static bool TryToLongExt(this string s, out long value)
    {
        return long.TryParse(s, out value);
    }

    public static bool TryToLongExt(this string s, int start, out long value)
    {
        if (s.GetCountExt() <= start)
        {
            value = default;
            return false;
        }

        return long.TryParse(s.AsSpan(start), out value);
    }

    public static long ToLongExt(this string s, long defaultValue = 0)
    {
        if (TryToLongExt(s, out var value) == false)
        {
            return defaultValue;
        }

        return value;
    }

    public static bool TryToUlongExt(this string s, out ulong value)
    {
        return ulong.TryParse(s, out value);
    }

    public static bool TryToUlongExt(this string s, int start, out ulong value)
    {
        if (s.GetCountExt() <= start)
        {
            value = default;
            return false;
        }

        return ulong.TryParse(s.AsSpan(start), out value);
    }

    public static ulong ToUlongExt(this string s, ulong defaultValue = 0)
    {
        if (TryToUlongExt(s, out var value) == false)
        {
            return defaultValue;
        }

        return value;
    }

    public static bool TryToFloatExt(this string s, out float value)
    {
        return float.TryParse(s, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out value);
    }

    public static bool TryToFloatExt(this string s, int start, out float value)
    {
        if (s.GetCountExt() <= start)
        {
            value = default;
            return false;
        }

        return float.TryParse(s.AsSpan(start), out value);
    }

    public static float ToFloatExt(this string s, float defaultValue = 0)
    {
        if (TryToFloatExt(s, out var value) == false)
        {
            return defaultValue;
        }

        return value;
    }

    public static bool TryToDoubleExt(this string s, out double value)
    {
        return double.TryParse(s, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out value);
    }

    public static bool TryToDoubleExt(this string s, int start, out double value)
    {
        if (s.GetCountExt() <= start)
        {
            value = default;
            return false;
        }

        return double.TryParse(s.AsSpan(start), out value);
    }

    public static double ToDoubleExt(this string s, double defaultValue = 0)
    {
        if (TryToDoubleExt(s, out var value) == false)
        {
            return defaultValue;
        }

        return value;
    }

    public static bool ToBoolExt(this string s, bool defaultValue = false)
    {
        if (s.IsEmptyExt())
        {
            return defaultValue;
        }

        if (s == "0")
        {
            return defaultValue;
        }

        if (string.Equals(s, "false", StringComparison.OrdinalIgnoreCase))
        {
            return defaultValue;
        }

        return true;
    }
    #endregion
    
    #region case string
    public static string ToUpperCaseStringExt(this string s)
    {
        var length = s.GetCountExt();
        if (length == 0)
        {
            return s;
        }

        var builder = new StringBuilder();
        for (var i = 0; i < length; i++)
        {
            var c = s[i];
            if (char.IsLetter(c))
            {
                if (char.IsLower(c))
                {
                    builder.Append(char.ToUpper(c));
                }
                else
                {
                    if (i > 0 && char.IsUpper(s[i - 1]) == false)
                    {
                        builder.Append('_');
                    }

                    builder.Append(c);
                }
            }
            else
            {
                builder.Append(c);
            }
        }

        return builder.ToString();
    }

    public static bool IgnoreCaseEqualsExt(this string s, string other)
    {
        return string.Equals(s, other, StringComparison.OrdinalIgnoreCase);
    }
    #endregion

    #region path operation
    public static bool DirectoryExistExt(this string s)
    {
        return IOUtil.DirectoryExist(s);
    }

    public static string GetDirectoryNameExt(this string s)
    {
        return IOUtil.GetDirectoryName(s);
    }

    public static bool FileExistExt(this string s)
    {
        return IOUtil.FileExist(s);
    }

    public static string GetExtensionExt(this string s)
    {
        return IOUtil.GetExtension(s);
    }

    public static string GetFileNameExt(this string s)
    {
        return IOUtil.GetFileName(s);
    }

    public static string GetFileNameWithoutExtensionExt(this string s)
    {
        return IOUtil.GetFileNameWithoutExtension(s);
    }

    public static string ReplaceExtensionExt(this string s, string extension)
    {
        if (s.IsEmptyExt())
        {
            return string.Empty;
        }

        var oldExtension = GetExtensionExt(s);
        if (oldExtension.IsEmptyExt())
        {
            return s + extension;
        }

        if (oldExtension == extension)
        {
            return s;
        }

        var replaced = s.Replace(oldExtension, extension);
        return replaced;
    }

    public static string RemoveExtensionExt(this string s)
    {
        return ReplaceExtensionExt(s, string.Empty);
    }

    public static bool HasExtensionExt(this string s, IEnumerable<string> extensions)
    {
        if (extensions == null)
        {
            return false;
        }

        var myExtension = GetExtensionExt(s);
        if (myExtension.IsEmptyExt())
        {
            return false;
        }

        foreach (var extension in extensions)
        {
            if (string.Equals(myExtension, extension, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    public static string PadRightExt(this string s, char padChar, int alignCount)
    {
        var currentLength = s != null ? s.Length : 0;
        var needPadCount = alignCount - currentLength;
        if (needPadCount <= 0)
        {
            return s;
        }

        var builder = new StringBuilder();
        if (s != null)
        {
            builder.Append(s);
        }

        builder.Append(padChar, needPadCount);
        return builder.ToString();
    }

    public static string CombinePathExt(this string s, params object[] paths)
    {
        return IOUtil.CombinePath(s, IOUtil.CombinePath(paths));
    }

    public static string GetFullPathExt(this string s)
    {
        return IOUtil.GetFullPath(s);
    }

    public static string AppendSlashExt(this string s, char slash = '/')
    {
        if (string.IsNullOrEmpty(s))
        {
            return $"{slash}";
        }

        if (s.EndsWith(slash))
        {
            return s;
        }

        return $"{s}{slash}";
    }

    public static string AppendEndIfNotExt(this string s, string end)
    {
        if (s == null)
        {
            return end;
        }

        if (s.EndsWith(end))
        {
            return s;
        }

        return $"{s}{end}";
    }

    public static string TrimExt(this string s)
    {
        if (s == null)
        {
            return s;
        }

        return s.Trim();
    }
    #endregion
}
