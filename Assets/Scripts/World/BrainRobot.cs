using UnityEngine;

public class BrainRobot : Robot
{
    public bool carrying;
    public Workspace workspace;

    public override void PickUp()
    {
        if (carrying)
            return;

        var shell = board.FindNear<Shell>(pos);
        if (shell == null)
            return;

        shell.Take();
        carrying = true;
    }

    public override void Give()
    {
        if (!carrying)
            return;

        var crab = board.FindNear<Crab>(pos);
        if (crab == null || !crab.Receive())
            return;

        carrying = false;
    }

    public override void Upload(string file)
    {
        if (workspace == null)
            return;

        var script = workspace.Find(file);
        if (script == null)
            return;

        var target = board.FindNear<Robot>(pos);
        if (target == null || target == this)
            return;

        var executor = target.GetComponent<CodeExecutor>();
        if (executor != null)
            executor.SetCommandList(CodeParsing.ParseText(script.Source));
    }

    public override EntityState Save()
    {
        var state = base.Save();
        state.carrying = carrying;
        return state;
    }

    public override void Restore(EntityState state)
    {
        carrying = state.carrying;
        base.Restore(state);
    }
}
