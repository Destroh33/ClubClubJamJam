using TMPro;
using UnityEngine;
using Unity;
using System.Collections.Generic;

public class CodeParsing : MonoBehaviour
{
    public TextMeshProUGUI codeText;
    public TextMeshProUGUI errorOutputText;
    public int lineOfExecution = 0;
    public int totalLines;

    public static List<string> commandList = new List<string>();

    [SerializeF]

    private void Awake()
    {
        commandList.Clear();
    }

    /// <summary>
    /// Code commands are separated purely by semicolons. There are no loops/conditionals, so 
    /// </summary>
    /// <param name="code"></param>
    public void ParseText(TextMeshProUGUI codeText) 
    {
        string code = codeText.text;

        while (code.Length != 0) 
        {
            int indSemicolonNext = code.IndexOf(';');

            if (indSemicolonNext != -1)
            {//LINE OF COMMAND
                string command = code.Substring(0, indSemicolonNext);
                command.Trim();
                commandList.Add(command);
            }
            else 
            {
                if (lineOfExecution < totalLines) 
                {
                    LogError("ERROR: missing semicolon");
                }
            }

            

        }
    }


    //new CommandInfo("up;", ArgumentKind.Number, "up(tiles)", "Move north the given number of tiles."),
    //    new CommandInfo("down;", ArgumentKind.Number, "down(tiles)", "Move south the given number of tiles."),
    //    new CommandInfo("left;", ArgumentKind.Number, "left(tiles)", "Move west the given number of tiles."),
    //    new CommandInfo("right;", ArgumentKind.Number, "right(tiles)", "Move east the given number of tiles."),
    //    new CommandInfo("useAbility;", ArgumentKind.None, "useAbility()", "Trigger whatever this bot is built to do."),
    //    new CommandInfo("wait;", ArgumentKind.Number, "wait(ticks)", "Do nothing for the given number of ticks."),
    //    new CommandInfo("upload;", ArgumentKind.FileName, "upload(file)", "Send a program to the bot in front. Main bot only.")

    /// <summary>
    /// <param name="command"></param>
    public void ExecuteCommand(string command) 
    {
        string argString =  "";
        int argAsNumber;
        if (command.IndexOf('(') != -1) 
        {
            argString = command.Substring(command.IndexOf('(')); //gets the argument
            argString.Trim();
            argString = argString.Trim('(', ')');
        }
        int.TryParse(argString, out argAsNumber); //

        string commandName = command.Substring(0, command.IndexOf("("));

        switch (commandName)  //just call the necessary command here
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
                LogError("Unknown command - check if there's a typo or missing semicolon");
                break;
        }
    }


    public void LogError(string errorText) 
    {
        errorOutputText.text = errorText;
    }

    
}
