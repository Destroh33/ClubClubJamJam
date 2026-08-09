using UnityEngine;

public class GameRoot : MonoBehaviour
{
    public CodePanel code;
    public TerminalPanel terminal;
    public LevelRunner runner;
    public CursorSet cursors = new CursorSet();

    public static CursorSet Cursors = new CursorSet();

    void Start()
    {
        Cursors = cursors;
        cursors.ShowPointer();

        if (GameSession.Selected != null)
            runner.level = GameSession.Selected;

        var workspace = runner.level != null ? runner.level.CreateWorkspace() : Workspace.CreateStarter();

        var brain = FindAnyObjectByType<BrainRobot>();
        if (brain != null)
            brain.workspace = workspace;

        runner.Bind(terminal);

        var registry = new CommandRegistry();
        TerminalCommands.RegisterAll(registry);

        terminal.Workspace = workspace;
        terminal.Runner = runner;
        terminal.Bind(registry, "shellsweep terminal");

        code.Bind(workspace);
    }
}
