using UnityEngine;

public class LevelRunner : MonoBehaviour
{
    const int BarWidth = 24;

    public Board board;
    public LevelConfig level;

    public SimulationClock Clock = new SimulationClock();
    public bool Started;
    public int ticks;

    TerminalPanel terminal;
    float timer;

    public int MaxTicks
    {
        get { return level != null ? level.maxTicks : 0; }
    }

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
        Started = true;
        timer = 0f;
        Report();
    }

    public void Reset()
    {
        while (board.UndoTick()) { }

        Clock.Running = false;
        Clock.Paused = false;
        Started = false;
        ticks = 0;
        terminal.ClearStatus();
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

        if (MaxTicks > 0 && ticks >= MaxTicks)
        {
            Clock.Running = false;
            terminal.PrintError("out of ticks, the level is lost");
            return;
        }

        board.Tick();
        ticks++;
        Report();
    }

    public void Back()
    {
        if (!board.UndoTick())
            return;

        ticks = Mathf.Max(0, ticks - 1);
        Report();
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

    void Report()
    {
        if (MaxTicks <= 0)
        {
            terminal.ClearStatus();
            return;
        }

        int used = Mathf.Clamp(ticks, 0, MaxTicks);
        int filled = Mathf.RoundToInt((float)used / MaxTicks * BarWidth);

        var bar = new System.Text.StringBuilder();
        bar.Append("\n[<color=#FFB066>");
        bar.Append('#', filled);
        bar.Append("</color><color=#3A4152>");
        bar.Append('-', BarWidth - filled);
        bar.Append("</color>]  ");
        bar.Append(MaxTicks - used);
        bar.Append(" of ");
        bar.Append(MaxTicks);
        bar.Append(" ticks left");

        terminal.SetStatus(bar.ToString());
    }
}
