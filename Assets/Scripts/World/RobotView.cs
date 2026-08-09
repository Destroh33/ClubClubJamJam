using UnityEngine;

public class RobotView : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Animator animator;
    public Color tint = Color.white;

    float walking;

    void Awake()
    {
        if (sprite == null)
            sprite = GetComponentInChildren<SpriteRenderer>();
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
        if (sprite != null)
            sprite.color = tint;
    }

    void Start()
    {
        Play("Idle");
    }

    void Update()
    {
        if (walking <= 0f)
            return;

        walking -= Time.deltaTime;
        if (walking <= 0f)
            Play("Idle");
    }

    public void Face(Vector2Int dir)
    {
        if (sprite != null && dir.x != 0)
            sprite.flipX = dir.x < 0;

        walking = 0.4f;
        Play("Walk");
    }

    void Play(string clip)
    {
        if (animator != null && animator.runtimeAnimatorController != null)
            animator.Play(clip, 0, 0f);
    }
}
