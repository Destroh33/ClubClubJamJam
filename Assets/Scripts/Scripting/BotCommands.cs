using System.Collections.Generic;

public enum ArgumentKind
{
    None,
    Number,
    FileName
}

public class CommandInfo
{
    public string Name;
    public ArgumentKind Argument;
    public string Usage;
    public string Summary;

    public CommandInfo(string name, ArgumentKind argument, string usage, string summary)
    {
        Name = name;
        Argument = argument;
        Usage = usage;
        Summary = summary;
    }
}

public static class BotCommands
{
    public static readonly CommandInfo[] All =
    {
        new CommandInfo("up", ArgumentKind.Number, "up(tiles);", "Move north the given number of tiles."),
        new CommandInfo("down", ArgumentKind.Number, "down(tiles);", "Move south the given number of tiles."),
        new CommandInfo("left", ArgumentKind.Number, "left(tiles);", "Move west the given number of tiles."),
        new CommandInfo("right", ArgumentKind.Number, "right(tiles);", "Move east the given number of tiles."),
        new CommandInfo("useAbility", ArgumentKind.None, "useAbility();", "Trigger whatever this bot is built to do."),
        new CommandInfo("wait", ArgumentKind.Number, "wait(ticks);", "Do nothing for the given number of ticks."),
        new CommandInfo("upload", ArgumentKind.FileName, "upload(file);", "Send a program to the bot in front. Main bot only.")
    };

    static readonly Dictionary<string, CommandInfo> lookup = BuildLookup();

    static Dictionary<string, CommandInfo> BuildLookup()
    {
        var table = new Dictionary<string, CommandInfo>();
        foreach (var command in All)
            table.Add(command.Name, command);
        return table;
    }

    public static CommandInfo Find(string name)
    {
        CommandInfo command;
        return lookup.TryGetValue(name, out command) ? command : null;
    }

    public static bool Exists(string name)
    {
        return lookup.ContainsKey(name);
    }
}
