using UnityEngine;

public class Door : Entity
{
    [SerializeField] public Channel channel;
    [SerializeField] Sprite closedSprite;
    [SerializeField] Sprite openSprite;
    
    public override bool IsSolid()
    {
        return !board.IsChannelActive(channel);
    }

    bool open;
    bool known;

    protected override void Update()
    {
        base.Update();

        bool nowOpen = !IsSolid();
        if (!known || nowOpen != open)
        {
            if (known)
                Sfx.Door(nowOpen);
            open = nowOpen;
            known = true;
        }

        spriteRenderer.sprite = nowOpen ? openSprite : closedSprite;
    }

}
