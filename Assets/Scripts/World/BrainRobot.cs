using UnityEngine;
using UnityEngine.InputSystem;

public class BrainRobot : Robot
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

    public void OnAttack()
    {
        UseAbility();
    }
}
