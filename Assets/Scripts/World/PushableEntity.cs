using UnityEngine;

public class PushableEntity : Entity
{
    public override bool IsPushable()
    {
        return true;
    }

    public override bool TryPush(Vector2Int dir)
    {
        if (!base.TryPush(dir))
            return false;

        if (board.At<Spike>(pos) != null)
        {
            alive = false;
            Init();
        }

        return true;
    }
}
