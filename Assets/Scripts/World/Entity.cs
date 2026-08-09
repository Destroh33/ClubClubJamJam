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
        pos = GetGridPos(transform.position);
        Init();
    }

    protected virtual void Update() {
        transform.position = Vector3.MoveTowards(transform.position, GetWorldPos(pos), animationSpeed * Time.deltaTime);
    }

    public virtual void Init()
    {
        transform.position = GetWorldPos(pos);
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

    public Vector3 GetWorldPos(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x, gridPos.y);
    }
    public Vector2Int GetGridPos(Vector3 worldPos)
    {
        return new Vector2Int(Mathf.RoundToInt(worldPos.x), Mathf.RoundToInt(worldPos.y));
    }

    public virtual void Tick() { }

    // public virtual void OnEnter(Entity other) { }
    public virtual bool TryPush(Vector2Int dir)
    {
        if (!IsPushable()) return false;

        Entity next = board.At(pos + dir);
        if (next == null)
        {
            pos = pos + dir;
            return true;
        }

        if (next.TryPush(dir))
        {
            pos = pos + dir;
            return true;
        }

        return false;
    }
}