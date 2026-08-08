using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using Unity.VisualScripting;


[Serializable]
public struct CommandToAction
{
    public string commandName;
    public UnityEvent commandAction;
}

public class CodeParsing : MonoBehaviour
{
    public TextMeshProUGUI codeText;
    public TextMeshProUGUI errorOutputText;
    public int lineOfExecution = 0;
    public int totalLines;

    public List<CommandToAction> commandList;
    private Dictionary<string, UnityEvent> commandsDict = new Dictionary<string, UnityEvent>();

    public List<string> executionList = new List<string>();

    

    private void Awake()
    {
        commandList.Clear();
        foreach (CommandToAction command in commandList) 
        {
            commandsDict.Add(command.commandName, command.commandAction);
        }
    }

    /// <summary>
    /// Code commands are separated purely by semicolons, each command is the block in between the semicolons
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
                command.ToLower();
                executionList.Add(command);
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
        else 
        {
            LogError("Error: function is missing parenthesis");
        }
        int.TryParse(argString, out argAsNumber);

        string commandName = command.Substring(0, command.IndexOf("("));

        if (!commandsDict.ContainsKey(commandName))
        {
            LogError("Error: invalid function");
            return;
        }

        commandsDict[commandName].Invoke();
    }

    void IncrementLineOfExecution(bool shouldIncrement) 
    {
        if(shouldIncrement)lineOfExecution++;
    }


    public void LogError(string errorText) 
    {
        errorOutputText.text = errorText;
    }

    
}
