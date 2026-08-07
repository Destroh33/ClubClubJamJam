using UnityEngine;

public class GameRoot : MonoBehaviour
{
    public CodePanel code;
    public TerminalPanel terminal;
    public CursorSet cursors = new CursorSet();

    public static CursorSet Cursors = new CursorSet();

    void Start()
    {
        Cursors = cursors;
        cursors.ShowPointer();

        var workspace = Workspace.CreateStarter();
        code.Bind(workspace);
        terminal.Bind(workspace, new SimulationClock());
    }
}
