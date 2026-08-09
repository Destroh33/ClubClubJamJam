using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

[Serializable]
public class CommandsListEntry
{
    public string name;
    public int arg;
    public int numTicksLeft;
}

public class CodeParsing : MonoBehaviour
{
    public TextMeshProUGUI errorOutputText;


    /// <summary>
    /// Code commands are separated purely by semicolons, each command is the block in between the semicolons
    /// </summary>
    /// <param name="codeText"></param> 
    public static List<CommandsListEntry> ParseText(TextMeshProUGUI codeText) 
    {
        List<string> commandList = new List<string>();

        string code = codeText.text;
        int count = 0;

        while (code.Length != 0) 
        {
            int indSemicolonNext = code.IndexOf(';');

            if (indSemicolonNext != -1)
            {//LINE OF COMMAND
                string command = code.Substring(0, indSemicolonNext);
                command = command.Trim();
                command = command.ToLower();
                commandList.Add(command);
            }
            count++;
            if (count > 500) break;
            
        }

        return ParseExListToCommandEntries(commandList);

    }


    /// <summary>
    /// helper function used by ParseText to get the struct based command list
    /// </summary>
    /// <param name="commandList"></param>
    /// <returns></returns>
    public static List<CommandsListEntry> ParseExListToCommandEntries(List<string> commandList) 
    {
        if(commandList == null)return null;
        List<CommandsListEntry> executionList = new List<CommandsListEntry>();
        for (int i = 0; i < commandList.Count; i++) 
        {
            string argString = "";
            int argAsNumber;
            if (commandList[i].IndexOf('(') != -1)
            {
                argString = commandList[i].Substring(commandList[i].IndexOf('(')); //gets the argument
                argString = argString.Trim();
                argString = argString.Trim('(', ')');
            }
            else
            {
                //LogError("Error: function is missing parenthesis");
            }
            int.TryParse(argString, out argAsNumber);

            string commandName = commandList[i].Substring(0, commandList[i].IndexOf("("));

            CommandsListEntry entry = new CommandsListEntry();
            entry.name = commandName;
            entry.arg = argAsNumber;
            entry.numTicksLeft = (commandName == "upload" || commandName == "") ? 1 : entry.numTicksLeft = argAsNumber; //1 tick for an upload command or no param
        }


        return executionList;
    }



    public void LogError(string errorText) 
    {
        errorOutputText.text = errorText;
    }

    
}
