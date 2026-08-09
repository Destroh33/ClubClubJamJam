using UnityEngine;

public static class MenuCommands
{
    public static MenuRoot Menu;

    public static void RegisterAll(CommandRegistry registry)
    {
        registry.Register("play", "play", "Start a new run from the first level.", Play);
        registry.Register("levels", "levels", "List every level.", Levels);
        registry.Register("level", "level <number>", "Load one level by number.", Level);
        registry.Register("credits", "credits", "Who made this.", Credits);
        registry.Register("help", "help [command]", "List the commands, or explain one of them.", Help);
        registry.Register("clear", "clear", "Wipe the terminal.", Clear);
        registry.Register("exit", "exit", "Quit the game.", Exit);
    }

    static void Play(string[] arguments, TerminalPanel terminal)
    {
        Load(1, terminal);
    }

    static void Levels(string[] arguments, TerminalPanel terminal)
    {
        terminal.PrintHeading("levels");

        for (int i = 0; i < Menu.levels.Length; i++)
        {
            var level = Menu.levels[i];
            terminal.PrintRow((i + 1).ToString(), level != null ? level.levelName : "missing");
        }
    }

    static void Level(string[] arguments, TerminalPanel terminal)
    {
        int number;
        if (arguments.Length == 0 || !int.TryParse(arguments[0], out number))
        {
            terminal.PrintError("which level? try levels to see them");
            return;
        }

        Load(number, terminal);
    }

    static void Load(int number, TerminalPanel terminal)
    {
        if (number < 1 || number > Menu.levels.Length)
        {
            terminal.PrintError("there is no level " + number);
            return;
        }

        var level = Menu.levels[number - 1];
        if (level == null)
        {
            terminal.PrintError("level " + number + " is not built yet");
            return;
        }

        terminal.PrintGood("loading " + level.levelName);
        Menu.Load(level);
    }

    static void Credits(string[] arguments, TerminalPanel terminal)
    {
        terminal.PrintHeading("credits");
        foreach (string line in Menu.credits)
            terminal.Print("  " + line);
    }

    static void Help(string[] arguments, TerminalPanel terminal)
    {
        if (arguments.Length == 0)
        {
            terminal.PrintHeading("commands");
            foreach (var command in terminal.Commands.All)
                terminal.PrintRow(command.Usage, command.Summary);
            return;
        }

        var match = terminal.Commands.Find(arguments[0]);
        if (match == null)
        {
            terminal.PrintError("nothing called " + arguments[0]);
            return;
        }

        terminal.PrintRow(match.Usage, match.Summary);
    }

    static void Clear(string[] arguments, TerminalPanel terminal)
    {
        terminal.Clear();
    }

    static void Exit(string[] arguments, TerminalPanel terminal)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
