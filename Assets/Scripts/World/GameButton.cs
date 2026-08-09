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

    public bool IsPressed()
    {
        foreach (var e in board.entities)
        {
            if (e.pos == pos && (e is Robot || e is PushableEntity))
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = openButton;
                return true;
            }
        }
        gameObject.GetComponent<SpriteRenderer>().sprite = closeButton;
        return false;
        
    }
}
