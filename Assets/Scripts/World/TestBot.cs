using UnityEngine;
using UnityEngine.InputSystem;

public class TestBot : Robot
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void UseAbility()
    {
        Debug.Log("ability used");
    }

    public override void AttachScript()
    {
        Debug.Log("attach script");
    }

    public void OnMove(InputValue value) 
    {
        Vector2 dir = value.Get<Vector2>().normalized;

        if (dir == new Vector2(1, 0))
        {
            Right();
        }
        else if (dir == new Vector2(-1, 0)) 
        {
            Left();
        }
        else if (dir == new Vector2(0, 1))
        {
            Up();
        }
        else if (dir == new Vector2(0, -1))
        {
            Down();
        }

    }

    public void OnAttack() 
    {
        UseAbility();
    }
}
