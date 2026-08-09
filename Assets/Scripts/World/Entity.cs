using UnityEngine;
using System.Collections.Generic;

public struct EntityState {
  public Vector2Int pos;
  public bool alive;
}


public class Entity : MonoBehaviour
{
    public Board board;

    [SerializeField] public Vector2Int pos;
    public bool alive = true;

    [SerializeField] public float animationSpeed = 10f;

    public virtual bool IsPushable()
    {
        return false;
    }

    protected virtual void Awake() {
        pos = Board.GetGridPos(transform.position);
        Init();
    }

    protected virtual void Update() {
        transform.position = Vector3.MoveTowards(transform.position, Board.GetWorldPos(pos), animationSpeed * Time.deltaTime);
    }

    public virtual void Init()
    {
        transform.position = Board.GetWorldPos(pos);
    }

    public EntityState Save()
    {
        return new EntityState {pos = pos, alive = alive};
    }

    public void Restore(EntityState state)
    {
        pos = state.pos;
        alive = state.alive;

        Init();
    }

    public virtual void Tick() { }

    // public virtual void OnEnter(Entity other) { }
    public virtual bool TryPush(Vector2Int dir)
    {
        if (!IsPushable()) return false;

        Tile nextTile = board.TileAt(pos + dir);
        if (nextTile != null && !nextTile.IsStandable())
        {
            return false;
        }

        Entity nextEntity = board.EntityAt(pos + dir);
        if (nextEntity == null)
        {
            pos += dir;
            return true;
        }

        if (nextEntity.TryPush(dir))
        {
            pos += dir;
            return true;
        }

        return false;
    }
}