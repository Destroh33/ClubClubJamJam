using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Board : MonoBehaviour
{
    public Entity[] entities;
    readonly Stack<EntityState[]> history = new();


    void Awake()
    {
        Init();
    }

    void Update()
    {

        // TODO FOR TESTING
        var player = entities[0];
        if (Keyboard.current != null)
        {
            if (Keyboard.current.zKey.wasPressedThisFrame) {
                UndoTick();
            }
            else if (Keyboard.current.wKey.wasPressedThisFrame) {
                Tick();
                player.TryPush(new Vector2Int(0, 1));
            }
            else if (Keyboard.current.aKey.wasPressedThisFrame) {
                Tick();
                player.TryPush(new Vector2Int(-1, 0));
            }
            else if (Keyboard.current.sKey.wasPressedThisFrame) {
                Tick();
                player.TryPush(new Vector2Int(0, -1));
            }
            else if (Keyboard.current.dKey.wasPressedThisFrame) {
                Tick();
                player.TryPush(new Vector2Int(1, 0));
            }
        }

    }

    public void Init()
    {
        entities = FindObjectsOfType<Entity>();
        foreach (var e in entities) e.board = this;

    }

    public void Snapshot()
    {
        var snap = new EntityState[entities.Length];
        for (int i = 0; i < entities.Length; i++) {
            snap[i] = entities[i].Save();
        }
        history.Push(snap);
    }

    public void UndoTick()
    {
        if (history.Count == 0) return;

        var snap = history.Pop();
        for (int i = 0; i < entities.Length; i++) {
            entities[i].Restore(snap[i]);
        }
    }

    public Entity At(Vector2Int pos)
    {
        foreach (Entity e in entities)
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
