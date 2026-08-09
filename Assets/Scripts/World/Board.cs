using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public static readonly Vector2Int[] Neighbours =
    {
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, -1)
    };

    public Tilemap ground;
    public Entity[] entities;
    
    public GameButton[] buttons;

    readonly Stack<EntityState[]> history = new();

    public static Vector3 GetWorldPos(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x, gridPos.y);
    }

    public static Vector2Int GetGridPos(Vector3 worldPos)
    {
        return new Vector2Int(Mathf.RoundToInt(worldPos.x), Mathf.RoundToInt(worldPos.y));
    }

    void Awake()
    {
        Init();
    }

    public void Init()
    {
        entities = FindObjectsByType<Entity>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var e in entities)
            e.board = this;

        buttons = FindObjectsByType<GameButton>(FindObjectsInactive.Include, FindObjectsSortMode.None);
    }

    public bool IsStandable(Vector2Int pos)
    {
        return ground != null && ground.HasTile(new Vector3Int(pos.x, pos.y, 0));
    }

    public bool IsChannelActive(Channel channel) {
        if (channel == Channel.None) return false;

        foreach (var b in buttons)
        {
            if (b.channel == channel && b.IsPressed()) return true;
        }
        return false;
    }

    public void Snapshot()
    {
        var snap = new EntityState[entities.Length];
        for (int i = 0; i < entities.Length; i++)
            snap[i] = entities[i].Save();
        history.Push(snap);
    }

    public bool UndoTick()
    {
        if (history.Count == 0)
            return false;

        var snap = history.Pop();
        for (int i = 0; i < entities.Length; i++)
            entities[i].Restore(snap[i]);

        return true;
    }

    public Entity EntityAt(Vector2Int pos)
    {
        foreach (Entity e in entities)
        {
            if (e.alive && e.IsSolid() && e.pos == pos)
                return e;
        }
        return null;
    }

    public Entity AnyAt(Vector2Int pos)
    {
        foreach (Entity e in entities)
        {
            if (e.alive && e.pos == pos)
                return e;
        }
        return null;
    }

    public T At<T>(Vector2Int pos) where T : Entity
    {
        foreach (Entity e in entities)
        {
            if (e.alive && e.pos == pos && e is T found)
                return found;
        }
        return null;
    }

    public T FindNear<T>(Vector2Int pos) where T : Entity
    {
        foreach (var dir in Neighbours)
        {
            foreach (Entity e in entities)
            {
                if (e.alive && e.pos == pos + dir && e is T found)
                    return found;
            }
        }
        return null;
    }

    public void Tick()
    {
        Snapshot();
        foreach (Entity e in entities)
            e.Tick();
    }
}
