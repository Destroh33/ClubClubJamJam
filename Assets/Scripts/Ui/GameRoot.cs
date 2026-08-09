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

        var workspace = runner.level != null ? runner.level.CreateWorkspace() : Workspace.CreateStarter();

        var brain = FindAnyObjectByType<BrainRobot>();
        if (brain != null)
            brain.workspace = workspace;

        runner.Bind(terminal);
        code.Bind(workspace);
        terminal.Bind(workspace, runner);
    }
}
