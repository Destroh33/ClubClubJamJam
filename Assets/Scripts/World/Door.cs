using UnityEngine;

public class Door : Entity
{
    [SerializeField] public Channel channel;
    
    public override bool IsSolid()
    {
        return !board.IsChannelActive(channel);
    }

}
