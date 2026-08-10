using Unity.VisualScripting;
using UnityEngine;

public class GameButton : Entity
{
    [SerializeField] public Sprite openButton;
    [SerializeField] public Sprite closeButton;


    [SerializeField] public Channel channel;

    public override bool IsSolid()
    {
        return false;
    }

    bool pressed;

    public bool IsPressed()
    {
        bool now = false;
        foreach (var e in board.entities)
        {
            if (e.alive && e.pos == pos && (e is Robot || e is PushableEntity))
            {
                now = true;
                break;
            }
        }

        if (now != pressed)
        {
            pressed = now;
            if (now)
                Sfx.ButtonPress();
        }

        spriteRenderer.sprite = now ? openButton : closeButton;
        return now;
    }
}
