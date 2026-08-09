using UnityEngine;
using System.Collections.Generic;

public static class TerminalCommands
{
    public static void RegisterAll(CommandRegistry registry)
    {
        registry.Register("help", "help [command]", "List the commands, or explain one of them.", Help);
        registry.Register("run", "run", "Start the level.", Run);
        registry.Register("pause", "pause", "Freeze the simulation where it is.", Pause);
        registry.Register("resume", "resume", "Carry on after a pause.", Resume);
        registry.Register("step", "step", "Advance the simulation by a single tick.", Step);
        registry.Register("backstep", "backstep", "Rewind the simulation by a single tick.", BackStep);
        registry.Register("speedup", "speedup [amount]", "Run the simulation faster.", SpeedUp);
        registry.Register("slowdown", "slowdown [amount]", "Run the simulation slower.", SlowDown);
        registry.Register("botlist", "botlist", "Show every bot on the level.", BotList);
        registry.Register("files", "files", "List the program files in the workspace.", Files);
        registry.Register("clear", "clear", "Wipe the terminal.", Clear);

        registry.Register("exit", "exit", "Quit the game.", Exit);
    }

    static void Help(string[] arguments, TerminalPanel terminal)
    {
        if (arguments.Length == 0)
        {
            terminal.PrintHeading("terminal commands");
            foreach (var command in terminal.Commands.All)
                terminal.PrintRow(command.Usage, command.Summary);

            terminal.PrintHeading("bot commands, for use inside program files");
            foreach (var command in BotCommands.All)
                terminal.PrintRow(command.Usage, command.Summary);
            return;
        }

        string name = arguments[0];

        var match = terminal.Commands.Find(name);
        if (match != null)
        {
            terminal.PrintRow(match.Usage, match.Summary);
            return;
        }

        var botCommand = BotCommands.Find(name);
        if (botCommand != null)
        {
            terminal.PrintRow(botCommand.Usage, botCommand.Summary);
            return;
        }

        terminal.PrintError("nothing called " + name);
    }

    static void Run(string[] arguments, TerminalPanel terminal)
    {

        terminal.PrintMuted("nothing to run yet");
        Workspace workspace = terminal.Workspace;
        ScriptFile file = workspace.Files[0];
        CodeExecutor brainExecutor = terminal.brainRobot.GetComponent<CodeExecutor>();
        List<CommandsListEntry> comList = CodeParsing.ParseText(file.Source);
        brainExecutor.SetCommandList(comList);
    }

    static void Pause(string[] arguments, TerminalPanel terminal)
    {
        if (!RequireRunning(terminal))
            return;
        terminal.Clock.Paused = true;
        terminal.Print("paused");
    }

    static void Resume(string[] arguments, TerminalPanel terminal)
    {
        if (!RequireRunning(terminal))
            return;
        terminal.Clock.Paused = false;
        terminal.Print("running at " + Speed(terminal));
    }

    static void Step(string[] arguments, TerminalPanel terminal)
    {
        if (!RequireRunning(terminal))
            return;
        terminal.Clock.Paused = true;
        terminal.PrintMuted("stepping needs the level, coming soon");
    }

    static void BackStep(string[] arguments, TerminalPanel terminal)
    {
        if (!RequireRunning(terminal))
            return;
        terminal.Clock.Paused = true;
        terminal.PrintMuted("rewinding needs the level, coming soon");
    }

    static void SpeedUp(string[] arguments, TerminalPanel terminal)
    {
        ChangeSpeed(arguments, terminal, true);
    }

    static void SlowDown(string[] arguments, TerminalPanel terminal)
    {
        ChangeSpeed(arguments, terminal, false);
    }

    static void ChangeSpeed(string[] arguments, TerminalPanel terminal, bool faster)
    {
        float amount = 2f;
        if (arguments.Length > 0 && !float.TryParse(arguments[0], out amount))
        {
            terminal.PrintError("that is not a number");
            return;
        }

        if (amount <= 0f)
        {
            terminal.PrintError("the amount has to be bigger than zero");
            return;
        }

        terminal.Clock.SetSpeed(faster ? terminal.Clock.Speed * amount : terminal.Clock.Speed / amount);
        terminal.Print("speed is now " + Speed(terminal));
    }

    static void BotList(string[] arguments, TerminalPanel terminal)
    {
        terminal.PrintMuted("no bots yet, the level is not built");
    }

    static void Files(string[] arguments, TerminalPanel terminal)
    {
        terminal.PrintHeading("program files");
        foreach (var file in terminal.Workspace.Files)
            terminal.PrintRow(file.Name, file.Source.Split('\n').Length + " lines");
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

    static bool RequireRunning(TerminalPanel terminal)
    {
        if (terminal.Clock.Running)
            return true;
        terminal.PrintError("nothing is running, type run first");
        return false;
    }

    static string Speed(TerminalPanel terminal)
    {
        return terminal.Clock.Speed.ToString("0.##") + "x";
    }
}
