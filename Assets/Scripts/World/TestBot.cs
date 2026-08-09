using UnityEngine;
using UnityEngine.InputSystem;

public class TestBot : Robot
{
    public void OnMove(InputValue value)
    {
        Vector2 dir = value.Get<Vector2>();

        if (dir.x > 0.5f)
            Right();
        else if (dir.x < -0.5f)
            Left();
        else if (dir.y > 0.5f)
            Up();
        else if (dir.y < -0.5f)
            Down();
    }

    public void OnAttack()
    {
        UseAbility();
    }
}
