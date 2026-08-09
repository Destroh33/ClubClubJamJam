using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Board : MonoBehaviour
{
    public Entity[] entities;
    public Tile[] tiles;
    readonly Stack<EntityState[]> entityHistory = new();
    readonly Stack<TileState[]> tileHistory = new();

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

    void Update()
    {

        // // TODO FOR TESTING
        // var player = entities[0];
        // if (Keyboard.current != null)
        // {
        //     if (Keyboard.current.zKey.wasPressedThisFrame) {
        //         UndoTick();
        //     }
        //     else if (Keyboard.current.wKey.wasPressedThisFrame) {
        //         Tick();
        //         player.TryPush(new Vector2Int(0, 1));
        //     }
        //     else if (Keyboard.current.aKey.wasPressedThisFrame) {
        //         Tick();
        //         player.TryPush(new Vector2Int(-1, 0));
        //     }
        //     else if (Keyboard.current.sKey.wasPressedThisFrame) {
        //         Tick();
        //         player.TryPush(new Vector2Int(0, -1));
        //     }
        //     else if (Keyboard.current.dKey.wasPressedThisFrame) {
        //         Tick();
        //         player.TryPush(new Vector2Int(1, 0));
        //     }
        // }

    }

    public void Init()
    {
        entities = FindObjectsByType<Entity>();
        foreach (var e in entities) e.board = this;

        tiles = FindObjectsByType<Tile>();
        foreach (var e in tiles) e.board = this;

    }

    public void Snapshot()
    {
        var entitySnap = new EntityState[entities.Length];
        for (int i = 0; i < entities.Length; i++) {
            entitySnap[i] = entities[i].Save();
        }
        entityHistory.Push(entitySnap);

        var tileSnap = new TileState[tiles.Length];
        for (int i = 0; i < tiles.Length; i++)
        {
            tileSnap[i] = tiles[i].Save();
        }
        tileHistory.Push(tileSnap);
    }

    public void UndoTick()
    {
        if (entityHistory.Count == 0 || tileHistory.Count == 0) return;

        var entitySnap = entityHistory.Pop();
        for (int i = 0; i < entities.Length; i++)
        {
            entities[i].Restore(entitySnap[i]);
        }

        var tileSnap = tileHistory.Pop();
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].Restore(tileSnap[i]);
        }
    }

    public Entity EntityAt(Vector2Int pos)
    {
        foreach (Entity e in entities)
        {
            if (e.pos == pos) return e;
        }
        return null;
    }

    public Tile TileAt(Vector2Int pos)
    {
        foreach (Tile e in tiles)
        {
            if (e.pos == pos) return e;
        }
        return null;
    }

    public void Tick()
    {
        Snapshot();

        foreach (Entity e in entities) e.Tick();
    }

}
