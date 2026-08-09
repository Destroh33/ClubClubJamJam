using System;
using System.Collections.Generic;

[Serializable]
public class CommandsListEntry
{
    public string name;
    public int arg;
    public string file;
    public int numTicksLeft;
}

public static class CodeParsing
{
    public static List<CommandsListEntry> ParseText(string code)
    {
        List<string> errors;
        return ParseText(code, out errors);
    }

    public static List<CommandsListEntry> ParseText(string code, out List<string> errors)
    {
        errors = new List<string>();
        var list = new List<CommandsListEntry>();
        if (string.IsNullOrEmpty(code))
            return list;

        foreach (string piece in Strip(code).Split(';'))
        {
            string text = piece.Trim();
            if (text.Length == 0)
                continue;

            var entry = ParseCommand(text, errors);
            if (entry != null)
                list.Add(entry);
        }

        return list;
    }

    static CommandsListEntry ParseCommand(string text, List<string> errors)
    {
        int open = text.IndexOf('(');
        int close = text.LastIndexOf(')');

        if (open < 0 || close < open)
        {
            errors.Add(text + " is missing its brackets");
            return null;
        }

        string name = text.Substring(0, open).Trim().ToLower();
        string arg = text.Substring(open + 1, close - open - 1).Trim();

        var info = BotCommands.Find(name);
        if (info == null)
        {
            errors.Add("there is no command called " + name);
            return null;
        }

        var entry = new CommandsListEntry();
        entry.name = name;
        entry.arg = 1;
        entry.numTicksLeft = 1;

        if (info.Argument == ArgumentKind.FileName)
        {
            if (arg.Length == 0)
            {
                errors.Add(name + " needs a file name");
                return null;
            }
            entry.file = arg;
            return entry;
        }

        if (info.Argument == ArgumentKind.Number)
        {
            int count;
            if (!int.TryParse(arg, out count))
            {
                errors.Add(name + " needs a number, not " + arg);
                return null;
            }
            if (count < 1)
            {
                errors.Add(name + " needs a number above zero");
                return null;
            }
            entry.arg = count;
            entry.numTicksLeft = count;
        }

        return entry;
    }

    static string Strip(string code)
    {
        var text = new System.Text.StringBuilder(code.Length);
        int i = 0;

        while (i < code.Length)
        {
            char c = code[i];

            if (c == '/' && i + 1 < code.Length && code[i + 1] == '/')
            {
                while (i < code.Length && code[i] != '\n')
                    i++;
                continue;
            }

            if (c == '{')
            {
                int back = text.Length;
                while (back > 0 && text[back - 1] != ';' && text[back - 1] != '\n')
                    back--;
                text.Length = back;
                i++;
                continue;
            }

            if (c == '}')
            {
                i++;
                continue;
            }

            text.Append(c);
            i++;
        }

        return text.ToString();
    }
}
