using UnityEngine;

public class GameButton : Entity
{

    [SerializeField] public Channel channel;

    public override bool IsSolid()
    {
        return false;
    }

    public bool IsPressed()
    {
        foreach (var e in board.entities)
        {
            if (e.pos == pos && (e is Robot || e is PushableEntity))
            {
                return true;
            }
        }
        return false;
    }
}
