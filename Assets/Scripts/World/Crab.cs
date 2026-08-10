using UnityEngine;

public class Crab : Entity
{
    public bool hasShell;
    public Animator animator;
    [SerializeField] GameObject thoughtBubble;

    protected override void Awake()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
        base.Awake();
    }

    void Start()
    {
        Refresh();
    }

    public override bool IsPushable()
    {
        return false;
    }

    public bool Receive()
    {
        thoughtBubble.SetActive(false);
        if (hasShell)
            return false;

        hasShell = true;
        Refresh();
        return true;
    }

    public override void Init()
    {
        base.Init();
        Refresh();
    }

    void Refresh()
    {
        if (animator != null && animator.runtimeAnimatorController != null)
            animator.Play(hasShell ? "idle_shell" : "idle_noshell", 0, 0f);
    }

    public override EntityState Save()
    {
        var state = base.Save();
        state.carrying = hasShell ? 1 : 0;
        return state;
    }

    public override void Restore(EntityState state)
    {
        hasShell = state.carrying > 0;
        base.Restore(state);
    }
}
