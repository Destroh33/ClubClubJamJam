using UnityEngine;

public class Shell : Entity
{
    public override bool IsPushable()
    {
        return false;
    }

    public override bool IsSolid()
    {
        return false;
    }

    public void Take()
    {
        alive = false;
        Init();
    }
}
