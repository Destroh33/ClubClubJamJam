using UnityEngine;

public struct EntityState
{
    public Vector2Int pos;
    public bool alive;
    public int command;
    public int ticks;
    public bool carrying;
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

    public virtual bool IsSolid()
    {
        return true;
    }

    protected virtual void Awake()
    {
        pos = Board.GetGridPos(transform.position);
        Init();
    }

    protected virtual void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, Board.GetWorldPos(pos), animationSpeed * Time.deltaTime);
    }

    public virtual void Init()
    {
        transform.position = Board.GetWorldPos(pos);
        gameObject.SetActive(alive);
    }

    public virtual EntityState Save()
    {
        return new EntityState { pos = pos, alive = alive };
    }

    public virtual void Restore(EntityState state)
    {
        pos = state.pos;
        alive = state.alive;
        Init();
    }

    public virtual void Tick() { }

    public virtual bool TryPush(Vector2Int dir)
    {
        if (!IsPushable())
            return false;

        if (!board.IsStandable(pos + dir))
            return false;

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
