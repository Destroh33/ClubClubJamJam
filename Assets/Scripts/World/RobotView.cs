using UnityEngine;

public class RobotView : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Animator animator;
    public string walkClip = "Walk";
    public string idleClip = "Idle";
    public string sleepClip = "";

    Robot robot;
    string playing;
    float walking;

    void Awake()
    {
        robot = GetComponent<Robot>();

        if (sprite == null)
            sprite = GetComponentInChildren<SpriteRenderer>();
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (walking > 0f)
        {
            walking -= Time.deltaTime;
            if (walking > 0f)
                return;
        }

        Play(Resting());
    }

    public void Face(Vector2Int dir)
    {
        if (sprite != null && dir.x != 0)
            sprite.flipX = dir.x < 0;

        walking = 0.4f;
        playing = null;
        Play(walkClip);
    }

    string Resting()
    {
        if (sleepClip.Length == 0)
            return idleClip;

        bool loaded = robot != null && robot.codeExecutor != null && robot.codeExecutor.commandsList.Count > 0;
        return loaded ? idleClip : sleepClip;
    }

    void Play(string clip)
    {
        if (clip == playing || clip.Length == 0)
            return;

        if (animator == null || animator.runtimeAnimatorController == null)
            return;

        playing = clip;
        animator.Play(clip, 0, 0f);
    }
}
