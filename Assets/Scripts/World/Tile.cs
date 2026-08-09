using UnityEngine;


public struct TileState {
  public Vector2Int pos;
}

public class Tile : MonoBehaviour
{
    public Board board;
    [SerializeField] public Vector2Int pos;

    public virtual bool IsStandable()
    {
        return true;
    }

    protected virtual void Awake() {
        pos = Board.GetGridPos(transform.position);
        Init();
    }

    public TileState Save()
    {
        return new TileState {pos = pos};
    }

    public void Restore(TileState state)
    {
        pos = state.pos;

        Init();
    }


    public virtual void Tick() { }

    public virtual void Init()
    {
        transform.position = Board.GetWorldPos(pos);
    }


}
