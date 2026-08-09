using UnityEngine;

public class LevelRunner : MonoBehaviour
{
    public Board board;
    public LevelConfig level;

    public SimulationClock Clock = new SimulationClock();

    TerminalPanel terminal;
    float timer;

    public void Bind(TerminalPanel panel)
    {
        terminal = panel;
        if (level != null)
            Clock.SetSpeed(level.startSpeed);
    }

    public void Run()
    {
        Clock.Running = true;
        Clock.Paused = false;
        timer = 0f;
    }

    void Update()
    {
        if (!Clock.Running || Clock.Paused)
            return;

        timer += Time.deltaTime;
        if (timer < Clock.SecondsPerTick)
            return;

        timer = 0f;
        Step();
    }

    public void Step()
    {
        if (Finished())
        {
            Resolve();
            return;
        }

        board.Tick();
    }

    public void Back()
    {
        board.UndoTick();
    }

    bool Finished()
    {
        foreach (var e in board.entities)
        {
            if (e is Robot robot && !robot.Done)
                return false;
        }
        return true;
    }

    public bool Solved()
    {
        foreach (var e in board.entities)
        {
            if (e is Crab crab && !crab.hasShell)
                return false;
        }
        return true;
    }

    void Resolve()
    {
        Clock.Running = false;

        if (Solved())
            terminal.PrintGood("level complete");
        else
            terminal.PrintError("level failed, type reset and try again");
    }
}
