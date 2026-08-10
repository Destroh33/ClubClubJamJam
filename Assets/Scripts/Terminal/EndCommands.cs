using UnityEngine;

public static class EndCommands
{
    public static EndRoot End;

    public static void RegisterAll(CommandRegistry registry)
    {
        registry.Register("y", "y", "Yes, take me to the secret level.", Yes);
        registry.Register("n", "n", "No thanks, back to the menu.", No);
        registry.Register("menu", "menu", "Back to the title screen.", No);
        registry.Register("help", "help", "List the commands.", Help);
        registry.Register("clear", "clear", "Wipe the terminal.", Clear);
        registry.Register("exit", "exit", "Quit the game.", Exit);
    }

    static void Yes(string[] arguments, TerminalPanel terminal)
    {
        if (End.Offer == null)
        {
            terminal.PrintError("there is nothing left to unlock");
            return;
        }

        terminal.PrintGood("loading " + End.Offer.levelName);
        End.Play(End.Offer);
    }

    static void No(string[] arguments, TerminalPanel terminal)
    {
        terminal.PrintMuted("back to the title");
        End.Menu();
    }

    static void Help(string[] arguments, TerminalPanel terminal)
    {
        terminal.PrintHeading("commands");
        foreach (var command in terminal.Commands.All)
            terminal.PrintRow(command.Usage, command.Summary);
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
