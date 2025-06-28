using System.Text;

public static class StringBuilderExt
{
    public static void AppendSlashExt(this StringBuilder s, string append)
    {
        if (s == null)
        {
            return;
        }

        if (s.Length > 0 && s[s.Length - 1] != StringExt.SlashChar)
        {
            s.Append(StringExt.SlashChar);
        }

        s.Append(append);
    }
}