using UnityEngine;

public class SimulationClock
{
    public const float BaseTicksPerSecond = 4f;
    public const float MinSpeed = 0.25f;
    public const float MaxSpeed = 8f;

    public float Speed = 1f;
    public bool Paused;
    public bool Running;

    public float TicksPerSecond
    {
        get { return BaseTicksPerSecond * Speed; }
    }

    public float SecondsPerTick
    {
        get { return 1f / TicksPerSecond; }
    }

    public void SetSpeed(float speed)
    {
        Speed = Mathf.Clamp(speed, MinSpeed, MaxSpeed);
    }

    public void Reset()
    {
        Speed = 1f;
        Paused = false;
        Running = false;
    }
}
