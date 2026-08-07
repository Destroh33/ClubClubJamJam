using System;
using System.Collections.Generic;

public class TerminalCommand
{
    public string Name;
    public string Usage;
    public string Summary;
    public Action<string[], TerminalPanel> Run;
}

public class CommandRegistry
{
    readonly List<TerminalCommand> commands = new List<TerminalCommand>();

    public List<TerminalCommand> All
    {
        get { return commands; }
    }

    public void Register(string name, string usage, string summary, Action<string[], TerminalPanel> run)
    {
        commands.Add(new TerminalCommand { Name = name, Usage = usage, Summary = summary, Run = run });
    }

    public TerminalCommand Find(string name)
    {
        foreach (var command in commands)
        {
            if (string.Equals(command.Name, name, StringComparison.OrdinalIgnoreCase))
                return command;
        }
        return null;
    }

    public List<TerminalCommand> StartingWith(string prefix)
    {
        var matches = new List<TerminalCommand>();
        foreach (var command in commands)
        {
            if (command.Name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                matches.Add(command);
        }
        return matches;
    }
}
