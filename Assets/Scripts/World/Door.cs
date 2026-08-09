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

    protected override void Update()
    {
        base.Update();

        if (IsSolid())
        {
            spriteRenderer.sprite = closedSprite;
        }
        else
        {
            spriteRenderer.sprite = openSprite;
        }
    }

}
