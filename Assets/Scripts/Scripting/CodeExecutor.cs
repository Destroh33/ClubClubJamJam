using System.Collections.Generic;
using UnityEngine;

public class CodeExecutor : MonoBehaviour
{
    public List<CommandsListEntry> commandsList = new List<CommandsListEntry>();
    public List<string> validCommandNames;
    public
    int currentCommand = 0;

    //new CommandInfo("up;", ArgumentKind.Number, "up(tiles)", "Move north the given number of tiles."),
    //    new CommandInfo("down;", ArgumentKind.Number, "down(tiles)", "Move south the given number of tiles."),
    //    new CommandInfo("left;", ArgumentKind.Number, "left(tiles)", "Move west the given number of tiles."),
    //    new CommandInfo("right;", ArgumentKind.Number, "right(tiles)", "Move east the given number of tiles."),
    //    new CommandInfo("useAbility;", ArgumentKind.None, "useAbility()", "Trigger whatever this bot is built to do."),
    //    new CommandInfo("wait;", ArgumentKind.Number, "wait(ticks)", "Do nothing for the given number of ticks."),
    //    new CommandInfo("upload;", ArgumentKind.FileName, "upload(file)", "Send a program to the bot in front. Main bot only.")

    /// <summary>
    /// <param name="command"></param>
    /// 

    public void ExecuteCommand()
    {
        if (currentCommand >= commandsList.Count) 
        {
            return;
            //end of execution
        }
        if (!validCommandNames.Contains(commandsList[currentCommand].name)) 
        {
            commandsList[currentCommand].numTicksLeft = 0;
            currentCommand++;
            return;
            //error
        }

        switch (commandsList[currentCommand].name) 
        {
            case "up":
                break;
            case "down":
                break;
            case "left":
                break;
            case "right":
                break;
            case "useAbility":
                break;
            case "wait":
                break;
            case "upload":
                break;

            default:
                break;
        }

        commandsList[currentCommand].numTicksLeft -= 1;
        if (commandsList[currentCommand].numTicksLeft <= 0) 
        {
            currentCommand++;
        }

    }
}
