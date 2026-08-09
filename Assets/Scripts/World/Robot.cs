using UnityEngine;
using System.Collections.Generic;

public class Robot : Entity
{
    public CodeExecutor codeExecutor;
    public Vector2Int facing = new Vector2Int(1, 0);

    protected RobotView view;

    protected override void Awake()
    {
        base.Awake();
        codeExecutor = GetComponent<CodeExecutor>();
        view = GetComponent<RobotView>();
    }

    public override void Tick()
    {
        if (alive && codeExecutor != null)
            codeExecutor.ExecuteCommand();
    }

    public bool Done
    {
        get { return !alive || codeExecutor == null || codeExecutor.Done; }
    }

    public bool Move(Vector2Int dir)
    {
        if (!alive)
            return false;

        facing = dir;
        if (view != null)
            view.Face(dir);

        Vector2Int target = pos + dir;

        Spike spike = board.At<Spike>(target);
        if (spike != null)
        {
            if (!CanCrush())
            {
                pos = target;
                Die();
                return true;
            }

            spike.alive = false;
            spike.Init();
        }

        Entity next = board.EntityAt(target);
        if (next != null && (next.IsSolid() && !next.TryPush(dir)))
            return false;

        if (!board.IsStandable(target))
        {
            pos = target;
            Die();
            return true;
        }

        pos = target;
        return true;
    }

    public void Die()
    {
        alive = false;
        Init();
    }

    public bool Up()
    {
        return Move(new Vector2Int(0, 1));
    }

    public bool Down()
    {
        return Move(new Vector2Int(0, -1));
    }

    public bool Left()
    {
        return Move(new Vector2Int(-1, 0));
    }

    public bool Right()
    {
        return Move(new Vector2Int(1, 0));
    }

    public virtual bool CanPush()
    {
        return false;
    }

    public virtual bool CanCrush()
    {
        return false;
    }

    public virtual void PickUp() { }

    public virtual void Give() { }

    public virtual void Upload(string file) { }

    public override EntityState Save()
    {
        var state = base.Save();
        if (codeExecutor != null)
        {
            state.currentCommand = codeExecutor.currentCommand;
            state.commandsList = new List<CommandsListEntry>(codeExecutor.commandsList);
            state.ticks = codeExecutor.Done ? 0 : codeExecutor.commandsList[codeExecutor.currentCommand].numTicksLeft;
        }
        return state;
    }

    public override void Restore(EntityState state)
    {
        base.Restore(state);
        if (codeExecutor == null)
            return;

        codeExecutor.currentCommand = state.currentCommand;
        codeExecutor.commandsList = new List<CommandsListEntry>(state.commandsList);
        if (!codeExecutor.Done)
            codeExecutor.commandsList[codeExecutor.currentCommand].numTicksLeft = state.ticks;
    }
}
