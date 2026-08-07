using System.Text;
using UnityEngine;

public static class SyntaxHighlighter
{
    public static string Highlight(string source, SyntaxTheme theme)
    {
        if (string.IsNullOrEmpty(source))
            return "";

        var output = new StringBuilder(source.Length * 2);
        bool insideBrackets = false;
        int i = 0;

        while (i < source.Length)
        {
            char c = source[i];

            if (c == '/' && i + 1 < source.Length && source[i + 1] == '/')
            {
                int end = source.IndexOf('\n', i);
                if (end < 0)
                    end = source.Length;
                Write(output, source.Substring(i, end - i), theme.comment);
                i = end;
                continue;
            }

            if (c == '\n')
            {
                output.Append('\n');
                insideBrackets = false;
                i++;
                continue;
            }

            if (char.IsLetter(c) || c == '_')
            {
                int start = i;
                while (i < source.Length && (char.IsLetterOrDigit(source[i]) || source[i] == '_'))
                    i++;
                string word = source.Substring(start, i - start);
                Write(output, word, WordColor(word, source, i, insideBrackets, theme));
                continue;
            }

            if (char.IsDigit(c))
            {
                int start = i;
                while (i < source.Length && char.IsDigit(source[i]))
                    i++;
                Write(output, source.Substring(start, i - start), theme.number);
                continue;
            }

            if (c == '(' || c == ')' || c == '{' || c == '}')
            {
                if (c == '(')
                    insideBrackets = true;
                else if (c == ')')
                    insideBrackets = false;
                Write(output, c.ToString(), theme.punctuation);
                i++;
                continue;
            }

            if (c == ' ' || c == '\t')
            {
                output.Append(c);
                i++;
                continue;
            }

            Write(output, c.ToString(), theme.plainText);
            i++;
        }

        return output.ToString();
    }

    static Color WordColor(string word, string source, int wordEnd, bool insideBrackets, SyntaxTheme theme)
    {
        if (insideBrackets)
            return theme.fileName;
        if (BotCommands.Exists(word))
            return theme.keyword;
        if (NextVisibleChar(source, wordEnd) == '{')
            return theme.programName;
        return theme.plainText;
    }

    static char NextVisibleChar(string source, int from)
    {
        for (int i = from; i < source.Length; i++)
        {
            if (source[i] != ' ' && source[i] != '\t')
                return source[i];
        }
        return '\0';
    }

    static void Write(StringBuilder output, string text, Color color)
    {
        output.Append("<color=#");
        output.Append(ColorUtility.ToHtmlStringRGB(color));
        output.Append('>');
        if (text.IndexOf('<') >= 0)
        {
            output.Append("<noparse>");
            output.Append(text);
            output.Append("</noparse>");
        }
        else
        {
            output.Append(text);
        }
        output.Append("</color>");
    }
}
