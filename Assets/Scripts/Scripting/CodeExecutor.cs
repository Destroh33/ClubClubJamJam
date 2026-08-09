using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Robot))]
public class CodeExecutor : MonoBehaviour
{
    public List<CommandsListEntry> commandsList = new List<CommandsListEntry>();
    public int currentCommand;

    Robot robot;

    void Awake()
    {
        robot = GetComponent<Robot>();
    }

    public bool Done
    {
        get { return currentCommand >= commandsList.Count; }
    }

    public void SetCommandList(List<CommandsListEntry> list)
    {
        commandsList = list;
        currentCommand = 0;
    }

    public void ClearCommandList()
    {
        commandsList.Clear();
        currentCommand = 0;
    }

    public void ExecuteCommand()
    {
        if (Done || !robot.alive)
            return;

        var entry = commandsList[currentCommand];

        switch (entry.name)
        {
            case "up":
                robot.Up();
                break;
            case "down":
                robot.Down();
                break;
            case "left":
                robot.Left();
                break;
            case "right":
                robot.Right();
                break;
            case "wait":
                break;
            case "pickup":
                robot.PickUp();
                break;
            case "give":
                robot.Give();
                break;
            case "upload":
                robot.Upload(entry.file);
                break;
        }

        entry.numTicksLeft--;
        if (entry.numTicksLeft <= 0)
            currentCommand++;
    }
}
