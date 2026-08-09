using UnityEngine;

public class Crab : Entity
{
    public bool hasShell;
    public Animator animator;

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
        state.carrying = hasShell;
        return state;
    }

    public override void Restore(EntityState state)
    {
        hasShell = state.carrying;
        base.Restore(state);
    }
}
