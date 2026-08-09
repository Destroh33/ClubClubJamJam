using UnityEngine;
using UnityEngine.SceneManagement;

public class Robot : Entity
{
    protected CodeExecutor codeExecutor;

    protected override void Awake()
    {
        base.Awake();
        codeExecutor = GetComponent<CodeExecutor>();
    }

    public bool Move(Vector2Int dir)
    {
        Entity next = board.EntityAt(pos + dir);
        Tile nextTile = board.TileAt(pos + dir);

        if (next is Spike)
        {
            Die();
        }
        if (!nextTile.IsStandable())
        {
            Die();
        }

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

    public void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public bool Up()
    {
        return Move(new Vector2Int(0, 1));
    }

    public bool Down()
    {
        return Move(new Vector2Int(0, -1));
    }

    public bool Left()
    {
        return Move(new Vector2Int(-1, 0));
    }

    public bool Right()
    {
        return Move(new Vector2Int(1, 0));
    }

    public virtual void UseAbility() {}

    public virtual void AttachScript() {}


}
