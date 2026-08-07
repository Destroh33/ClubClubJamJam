using UnityEngine;
using System.Collections.Generic;

public enum EntityType { Robot, Box, Wall }

public struct EntityState {
  public Vector2Int pos;
  public EntityType type;
  public bool alive;
}


public class Entity : MonoBehaviour
{
    public Board board;
    public EntityType type;
    public bool alive = true;
    public bool isPushable => type == EntityType.Box;

    [SerializeField] public Vector2Int pos;
    [SerializeField] public float animationSpeed = 10f;


    void Awake() {
        Init();
        type = EntityType.Box;
    }

    void Update() {
        transform.position = Vector3.MoveTowards(transform.position, GetWorldPos(pos), animationSpeed * Time.deltaTime);
    }

    public void Init()
    {
        transform.position = GetWorldPos(pos);
    }

    public EntityState Save()
    {
        return new EntityState {pos = pos, type = type, alive = alive};
    }

    public void Restore(EntityState state)
    {
        pos = state.pos;
        type = state.type;
        alive = state.alive;

        Init();
    }

    public Vector3 GetWorldPos(Vector2Int GridPos)
    {
        return new Vector3(GridPos.x, GridPos.y);
    }

    public virtual void Tick() { }

    protected void Move(Vector2Int newPos)
    {
        pos = newPos;
    }

    public bool CanPush(Vector2Int dir)
    {
        if (!isPushable) return false;

        Entity next = board.At(pos + dir);
        if (next == null) return true;

        return next.CanPush(dir);
    }

    public bool TryPush(Vector2Int dir)
    {
        if (!isPushable) return false;

        Entity next = board.At(pos + dir);
        if (next == null)
        {
            Move(pos + dir);
            return true;
        }

        if (next.TryPush(dir))
        {
            Move(pos + dir);
            return true;
        }

        return false;
    }
}